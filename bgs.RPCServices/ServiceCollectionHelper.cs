using System;
using System.Collections.Generic;

namespace bgs.RPCServices
{
	public sealed class ServiceCollectionHelper
	{
		private Dictionary<uint, ServiceDescriptor> importedServices = new Dictionary<uint, ServiceDescriptor>();

		private Dictionary<uint, ServiceDescriptor> exportedServices = new Dictionary<uint, ServiceDescriptor>();

		public Dictionary<uint, ServiceDescriptor> ImportedServices
		{
			get
			{
				return this.importedServices;
			}
		}

		public void AddImportedService(uint serviceId, ServiceDescriptor serviceDescriptor)
		{
			this.importedServices.Add(serviceId, serviceDescriptor);
		}

		public void AddExportedService(uint serviceId, ServiceDescriptor serviceDescriptor)
		{
			this.exportedServices.Add(serviceId, serviceDescriptor);
		}

		public ServiceDescriptor GetImportedServiceById(uint service_id)
		{
			if (this.importedServices == null)
			{
				return null;
			}
			ServiceDescriptor result;
			this.importedServices.TryGetValue(service_id, ref result);
			return result;
		}

		public ServiceDescriptor GetImportedServiceByName(string name)
		{
			using (Dictionary<uint, ServiceDescriptor>.Enumerator enumerator = this.importedServices.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<uint, ServiceDescriptor> current = enumerator.get_Current();
					if (current.get_Value().Name == name)
					{
						return current.get_Value();
					}
				}
			}
			return null;
		}

		public ServiceDescriptor GetExportedServiceById(uint service_id)
		{
			ServiceDescriptor result;
			this.exportedServices.TryGetValue(service_id, ref result);
			return result;
		}
	}
}
