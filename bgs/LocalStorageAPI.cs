using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace bgs
{
	public class LocalStorageAPI : BattleNetAPI
	{
		public delegate void DownloadCompletedCallback(byte[] data, object userContext);

		private static int m_depotport = 1119;

		private static List<LocalStorageFileState> m_completedDownloads = new List<LocalStorageFileState>();

		private static readonly int m_bufferSize = 1024;

		private static LogThreadHelper s_log = new LogThreadHelper("LocalStorage");

		private static int m_downloadId = 0;

		public int DepotPort
		{
			get
			{
				return LocalStorageAPI.m_depotport;
			}
			set
			{
				LocalStorageAPI.m_depotport = value;
			}
		}

		public LocalStorageAPI(BattleNetCSharp battlenet) : base(battlenet, "LocalStorage")
		{
		}

		public bool GetFile(ContentHandle ch, LocalStorageAPI.DownloadCompletedCallback cb, object userContext = null)
		{
			try
			{
				LocalStorageFileState localStorageFileState = new LocalStorageFileState(LocalStorageAPI.m_downloadId);
				localStorageFileState.CH = ch;
				localStorageFileState.Callback = cb;
				localStorageFileState.UserContext = userContext;
				LocalStorageAPI.s_log.LogDebug("Starting GetFile State={0}", new object[]
				{
					localStorageFileState
				});
				if (!this.LoadStateFromDrive(localStorageFileState))
				{
					LocalStorageAPI.s_log.LogDebug("Unable to load file from disk, starting a download. State={0}", new object[]
					{
						localStorageFileState
					});
					this.DownloadFromDepot(localStorageFileState);
				}
			}
			catch (Exception ex)
			{
				LocalStorageAPI.s_log.LogWarning("EXCEPTION (GetFile): {0}", new object[]
				{
					ex.get_Message()
				});
				return false;
			}
			return true;
		}

		public override void Process()
		{
			List<LocalStorageFileState> completedDownloads = LocalStorageAPI.m_completedDownloads;
			lock (completedDownloads)
			{
				using (List<LocalStorageFileState>.Enumerator enumerator = LocalStorageAPI.m_completedDownloads.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LocalStorageFileState current = enumerator.get_Current();
						if (current.FileData != null)
						{
							LocalStorageAPI.s_log.LogDebug("Request completed State={0}", new object[]
							{
								current
							});
						}
						else
						{
							LocalStorageAPI.s_log.LogWarning("Request failed State={0}", new object[]
							{
								current
							});
						}
						LocalStorageAPI.s_log.Process();
						current.Callback(current.FileData, current.UserContext);
					}
				}
				LocalStorageAPI.m_completedDownloads.Clear();
			}
		}

		private bool LoadStateFromDrive(LocalStorageFileState state)
		{
			try
			{
				LocalStorageAPI.s_log.LogDebug("LoadState State={0}", new object[]
				{
					state
				});
				string text = LocalStorageAPI.MakeFullPathFromState(state);
				LocalStorageAPI.s_log.LogDebug("Attempting to load {0}", new object[]
				{
					text
				});
				if (!File.Exists(text))
				{
					LocalStorageAPI.s_log.LogDebug("File does not exist, unable to load from disk.");
					bool result = false;
					return result;
				}
				FileStream fileStream = File.OpenRead(text);
				byte[] array = new byte[fileStream.get_Length()];
				fileStream.Read(array, 0, array.Length);
				fileStream.Close();
				if (LocalStorageAPI.ComputeSHA256(array) != state.CH.Sha256Digest)
				{
					LocalStorageAPI.s_log.LogDebug("File was loaded but integrity check failed, attempting to delete ...");
					File.Delete(text);
					bool result = false;
					return result;
				}
				LocalStorageAPI.DecompressStateIfPossible(state, array);
				LocalStorageAPI.s_log.LogDebug("Loading completed");
				LocalStorageAPI.FinalizeState(state);
			}
			catch (Exception ex)
			{
				LocalStorageAPI.s_log.LogWarning("EXCEPTION: {0}", new object[]
				{
					ex.get_Message()
				});
				bool result = false;
				return result;
			}
			return true;
		}

		private static void StoreStateToDrive(LocalStorageFileState state)
		{
			try
			{
				LocalStorageAPI.s_log.LogDebug("StoreState State={0}", new object[]
				{
					state
				});
				string text = LocalStorageAPI.MakeFullPathFromState(state);
				LocalStorageAPI.s_log.LogDebug("Attempting to save {0}", new object[]
				{
					text
				});
				if (File.Exists(text))
				{
					LocalStorageAPI.s_log.LogDebug("Unable to save the file, it already exists");
				}
				else
				{
					FileStream fileStream = File.Create(text, state.FileData.Length);
					if (state.CompressedData == null)
					{
						LocalStorageAPI.s_log.LogDebug("Writing uncompressed file to disk");
						fileStream.Write(state.FileData, 0, state.FileData.Length);
					}
					else
					{
						LocalStorageAPI.s_log.LogDebug("Writing compressed file to disk");
						fileStream.Write(state.CompressedData, 0, state.CompressedData.Length);
					}
					fileStream.Flush();
					fileStream.Close();
					LocalStorageAPI.s_log.LogDebug("Writing completed");
				}
			}
			catch (Exception ex)
			{
				LocalStorageAPI.s_log.LogWarning("EXCEPTION (StoreStateToDrive): {0}", new object[]
				{
					ex.get_Message()
				});
			}
		}

		private static string MakeFullPathFromState(LocalStorageFileState state)
		{
			string temporaryCachePath = BattleNet.Client().GetTemporaryCachePath();
			string text = state.CH.Sha256Digest + "." + state.CH.Usage;
			return Path.Combine(temporaryCachePath, text);
		}

		private void DownloadFromDepot(LocalStorageFileState state)
		{
			string text = string.Format("{0}.depot.battle.net", state.CH.Region);
			LocalStorageAPI.s_log.LogDebug("Starting download from {0}", new object[]
			{
				text
			});
			state.Connection.LogDebug = new Action<string>(LocalStorageAPI.s_log.LogDebug);
			state.Connection.LogWarning = new Action<string>(LocalStorageAPI.s_log.LogWarning);
			state.Connection.OnFailure = delegate
			{
				LocalStorageAPI.ExecuteFailedDownload(state);
			};
			state.Connection.OnSuccess = delegate
			{
				LocalStorageAPI.ConnectCallback(state);
			};
			state.Connection.Connect(text, this.DepotPort);
		}

		private static void ConnectCallback(LocalStorageFileState state)
		{
			try
			{
				LocalStorageAPI.s_log.LogDebug("ConnectCallback called for State={0}", new object[]
				{
					state
				});
				state.ReceiveBuffer = new byte[LocalStorageAPI.m_bufferSize];
				state.Socket.BeginReceive(state.ReceiveBuffer, 0, LocalStorageAPI.m_bufferSize, 0, new AsyncCallback(LocalStorageAPI.ReceiveCallback), state);
				string text = string.Format("GET /{0}.{1} HTTP/1.1\r\n", state.CH.Sha256Digest, state.CH.Usage);
				text = text + "Host: " + state.Connection.Host + "\r\n";
				text += "User-Agent: HearthStone\r\n";
				text += "Connection: close\r\n";
				text += "\r\n";
				byte[] bytes = Encoding.get_ASCII().GetBytes(text);
				state.Socket.Send(bytes, 0, bytes.Length, 0);
			}
			catch (Exception ex)
			{
				LocalStorageAPI.s_log.LogWarning("EXCEPTION: {0}", new object[]
				{
					ex.get_Message()
				});
			}
		}

		private static void ReceiveCallback(IAsyncResult ar)
		{
			LocalStorageFileState localStorageFileState = (LocalStorageFileState)ar.get_AsyncState();
			try
			{
				LocalStorageAPI.s_log.LogDebug("ReceiveCallback called for State={0}", new object[]
				{
					localStorageFileState
				});
				int num = localStorageFileState.Socket.EndReceive(ar);
				if (num > 0)
				{
					int num2 = (localStorageFileState.FileData != null) ? localStorageFileState.FileData.Length : 0;
					int num3 = num2 + num;
					MemoryStream memoryStream = new MemoryStream(new byte[num3], 0, num3, true, true);
					if (localStorageFileState.FileData != null)
					{
						memoryStream.Write(localStorageFileState.FileData, 0, localStorageFileState.FileData.Length);
					}
					memoryStream.Write(localStorageFileState.ReceiveBuffer, 0, num);
					localStorageFileState.FileData = memoryStream.GetBuffer();
					localStorageFileState.Socket.BeginReceive(localStorageFileState.ReceiveBuffer, 0, LocalStorageAPI.m_bufferSize, 0, new AsyncCallback(LocalStorageAPI.ReceiveCallback), localStorageFileState);
				}
				else
				{
					LocalStorageAPI.CompleteDownload(localStorageFileState);
				}
			}
			catch (Exception ex)
			{
				LocalStorageAPI.s_log.LogWarning("EXCEPTION: {0}", new object[]
				{
					ex.get_Message()
				});
				localStorageFileState.FailureMessage = ex.get_Message();
				LocalStorageAPI.ExecuteFailedDownload(localStorageFileState);
			}
		}

		private static void CompleteDownload(LocalStorageFileState state)
		{
			bool flag = true;
			LocalStorageAPI.s_log.LogDebug("Download completed for State={0}", new object[]
			{
				state
			});
			HTTPHeader hTTPHeader = LocalStorageAPI.ParseHTTPHeader(state.FileData);
			if (hTTPHeader == null)
			{
				LocalStorageAPI.s_log.LogWarning("Parsinig of HTTP header failed for State={0}", new object[]
				{
					state
				});
			}
			else
			{
				byte[] array = new byte[hTTPHeader.ContentLength];
				Array.Copy(state.FileData, hTTPHeader.ContentStart, array, 0, hTTPHeader.ContentLength);
				if (LocalStorageAPI.ComputeSHA256(array) != state.CH.Sha256Digest)
				{
					LocalStorageAPI.s_log.LogWarning("Integrity check failed for State={0}", new object[]
					{
						state
					});
				}
				else
				{
					flag = false;
					LocalStorageAPI.DecompressStateIfPossible(state, array);
				}
			}
			if (flag || state.FileData == null)
			{
				LocalStorageAPI.ExecuteFailedDownload(state);
			}
			else
			{
				LocalStorageAPI.ExecuteSucessfulDownload(state);
			}
		}

		private static void DecompressStateIfPossible(LocalStorageFileState state, byte[] data)
		{
			ulong num;
			if (LocalStorageAPI.IsCompressedStream(data, out num))
			{
				state.CompressedData = data;
				MemoryStream ms = new MemoryStream(data, 16, data.Length - 16);
				state.FileData = LocalStorageAPI.Inflate(ms, (int)num);
			}
			else
			{
				state.FileData = data;
			}
		}

		private static bool IsCompressedStream(byte[] data, out ulong decompressedLength)
		{
			decompressedLength = 0uL;
			try
			{
				if (data.Length < 16)
				{
					bool result = false;
					return result;
				}
				if (BitConverter.ToUInt32(data, 0) != 1131245658u)
				{
					bool result = false;
					return result;
				}
				if (BitConverter.ToUInt32(data, 4) != 0u)
				{
					bool result = false;
					return result;
				}
				decompressedLength = BitConverter.ToUInt64(data, 8);
			}
			catch (Exception ex)
			{
				LocalStorageAPI.s_log.LogWarning("EXCEPTION: {0}", new object[]
				{
					ex.get_Message()
				});
				bool result = false;
				return result;
			}
			return true;
		}

		private static HTTPHeader ParseHTTPHeader(byte[] data)
		{
			try
			{
				int num = LocalStorageAPI.SearchForBytePattern(data, new byte[]
				{
					13,
					10,
					13,
					10
				});
				HTTPHeader result;
				if (num == -1)
				{
					result = null;
					return result;
				}
				int num2 = num + 1;
				if (num2 >= data.Length)
				{
					result = null;
					return result;
				}
				string @string = Encoding.get_ASCII().GetString(data, 0, num);
				if (@string.IndexOf("200 OK") == -1)
				{
					result = null;
					return result;
				}
				Regex regex = new Regex("(?<=Content-Length:\\s)\\d+", 1);
				Match match = regex.Match(@string);
				if (!match.get_Success())
				{
					result = null;
					return result;
				}
				int num3 = (int)uint.Parse(match.get_Value());
				int num4 = data.Length - num2;
				if (num3 != num4)
				{
					result = null;
					return result;
				}
				result = new HTTPHeader
				{
					ContentLength = num3,
					ContentStart = num2
				};
				return result;
			}
			catch (Exception ex)
			{
				LocalStorageAPI.s_log.LogWarning("EXCEPTION (ParseHTTPHeader): {0}", new object[]
				{
					ex.get_Message()
				});
			}
			return null;
		}

		private static int SearchForBytePattern(byte[] source, byte[] pattern)
		{
			for (int i = 0; i < source.Length; i++)
			{
				if (pattern[0] == source[i] && source.Length - i >= pattern.Length)
				{
					bool flag = true;
					int num = 1;
					while (num < pattern.Length && flag)
					{
						if (source[i + num] != pattern[num])
						{
							flag = false;
						}
						num++;
					}
					if (flag)
					{
						return i + (pattern.Length - 1);
					}
				}
			}
			return -1;
		}

		private static void ExecuteFailedDownload(LocalStorageFileState state)
		{
			state.FileData = null;
			LocalStorageAPI.FinalizeDownload(state);
		}

		private static void ExecuteSucessfulDownload(LocalStorageFileState state)
		{
			LocalStorageAPI.StoreStateToDrive(state);
			LocalStorageAPI.FinalizeDownload(state);
		}

		private static void FinalizeDownload(LocalStorageFileState state)
		{
			state.Connection.Disconnect();
			state.ReceiveBuffer = null;
			LocalStorageAPI.FinalizeState(state);
		}

		private static void FinalizeState(LocalStorageFileState state)
		{
			List<LocalStorageFileState> completedDownloads = LocalStorageAPI.m_completedDownloads;
			lock (completedDownloads)
			{
				LocalStorageAPI.m_completedDownloads.Add(state);
			}
		}

		private static string ComputeSHA256(byte[] bytes)
		{
			SHA256 sHA = SHA256.Create();
			byte[] array = sHA.ComputeHash(bytes, 0, bytes.Length);
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				byte b = array2[i];
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		private static byte[] Inflate(MemoryStream ms, int length)
		{
			ms.Seek(0L, 0);
			Inflater inflater = new Inflater(false);
			InflaterInputStream inflaterInputStream = new InflaterInputStream(ms, inflater);
			byte[] array = new byte[length];
			int num = 0;
			int num2 = array.Length;
			try
			{
				while (true)
				{
					int num3 = inflaterInputStream.Read(array, num, num2);
					if (num3 <= 0)
					{
						break;
					}
					num += num3;
					num2 -= num3;
				}
			}
			catch (Exception ex)
			{
				LocalStorageAPI.s_log.LogWarning("EXCEPTION (Inflate): {0}", new object[]
				{
					ex.get_Message()
				});
				byte[] result = null;
				return result;
			}
			if (num != length)
			{
				LocalStorageAPI.s_log.LogWarning("Decompressed size does not equal expected size.");
				return null;
			}
			return array;
		}
	}
}
