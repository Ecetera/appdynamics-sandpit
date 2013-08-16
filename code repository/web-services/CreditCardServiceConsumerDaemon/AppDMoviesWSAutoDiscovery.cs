using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Globalization;
using System.Collections.ObjectModel;
using System.CodeDom.Compiler;


namespace CreditCardServiceConsumerDaemon
{
    public class AppDMoviesWSAutoDiscovery
    {
        object proxyinstance;

        public object InvokeMethod(string operationName, object[] operationParameters)
        {
            // Get the operation's method, invoke it, and get the return value
            object retVal = proxyinstance.GetType().GetMethod(operationName).Invoke(proxyinstance, operationParameters);
            return retVal;
        }
        public AppDMoviesWSAutoDiscovery(string webserviceBindingUri, string interfaceContractName)
        {
            // Define the metadata address, contract name, operation name, and parameters. 
            // You can choose between MEX endpoint and HTTP GET by changing the address and enum value.
            Uri mexAddress = new Uri(webserviceBindingUri);
            // For MEX endpoints use a MEX address and a mexMode of .MetadataExchange
            MetadataExchangeClientMode mexMode = MetadataExchangeClientMode.HttpGet;
            string contractName = interfaceContractName;

            //string creditcard, string expiry, string cvv

            // Get the metadata file from the service.
            MetadataExchangeClient mexClient = new MetadataExchangeClient(mexAddress, mexMode);
            mexClient.ResolveMetadataReferences = true;
            MetadataSet metaSet = mexClient.GetMetadata();

            // Import all contracts and endpoints
            WsdlImporter importer = new WsdlImporter(metaSet);
            Collection<ContractDescription> contracts = importer.ImportAllContracts();
            ServiceEndpointCollection allEndpoints = importer.ImportAllEndpoints();

            // Generate type information for each contract
            ServiceContractGenerator generator = new ServiceContractGenerator();
            var endpointsForContracts = new Dictionary<string, IEnumerable<ServiceEndpoint>>();

            foreach (ContractDescription contract in contracts)
            {
                generator.GenerateServiceContractType(contract);
                // Keep a list of each contract's endpoints
                endpointsForContracts[contract.Name] = allEndpoints.Where(
                    se => se.Contract.Name == contract.Name).ToList();
            }

            if (generator.Errors.Count != 0)
                throw new Exception("There were errors during code compilation.");

            // Generate a code file for the contracts 
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("C#");

            // Compile the code file to an in-memory assembly
            // Don't forget to add all WCF-related assemblies as references
            CompilerParameters compilerParameters = new CompilerParameters(
                new string[] { 
                    "System.dll", "System.ServiceModel.dll", 
                    "System.Runtime.Serialization.dll" });
            compilerParameters.GenerateInMemory = true;

            CompilerResults results = codeDomProvider.CompileAssemblyFromDom(compilerParameters, generator.TargetCompileUnit);
            if (results.Errors.Count > 0)
            {
                throw new Exception("There were errors during generated code compilation");
            }
            else
            {
                // Find the proxy type that was generated for the specified contract
                // (identified by a class that implements the contract and ICommunicationObject)
                Type[] mytypes = results.CompiledAssembly.GetTypes();

                Type clientProxyType = mytypes.First(
                    t => t.IsClass &&
                        t.GetInterface(contractName) != null &&
                        t.GetInterface(typeof(ICommunicationObject).Name) != null);

                // Get the first service endpoint for the contract
                ServiceEndpoint se = endpointsForContracts[contractName].First();

                // Create an instance of the proxy
                // Pass the endpoint's binding and address as parameters
                // to the ctor
                proxyinstance = results.CompiledAssembly.CreateInstance(
                    clientProxyType.Name,
                    false,
                    System.Reflection.BindingFlags.CreateInstance,
                    null,
                    new object[] { se.Binding, se.Address },
                    CultureInfo.CurrentCulture, null);
            }
        }
    }
}