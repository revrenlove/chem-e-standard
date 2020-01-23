# EFC.AgGateway.Integration

This project contains the service and all needed resources and functionality for communicating with an AgGateway web service.

Any questions can be directed to Jim Elrod (jelrod@vaco.com).

## Table of Contents

* [Intended Usage](#intended-usage)
  * [DTOs](#dtos)
  * [Mapping](#mapping)
  * [`IAgGatewayService` Interface](#iaggatewayservice-interface)
  * [Operations Supplied From the Standard `WSDLs`](#operations-supplied-from-the-standard-wsdls)
  * [Operations Supplied From the AgGateway DocExchange Generic `WSDL`](#operations-supplied-from-the-aggateway-docexchange-generic-wsdl)
* [Directory Structure](#directory-structure)
  * [Exceptions](#exceptions)
    * [`AgGatewayProtocolException`](#-aggatewayprotocolexception-)
  * [Models](#models)
    * [Partials](#partials)
  * [Security](#security)
  * [Specifications](#specifications)
  * [Utility](#utility)

## Intended Usage

Once a method has been defined in the `AgGatewayService`, it should be able to be used in outside classes as simply as:

```csharp
var service = AgGatewayServiceFactory.GetAgGatewayService(vendor);

var seedItems = service.GetImportSeedItems(priceSheetRequest);
```

The static `GetAgGatewayService` method resolves the particular service needed based upon the `Vendor` object passed in.

### DTOs

Any new DTOs that are needed (and are *strictly* used within AgGateway web service communication functionality) should be placed within the `Models` folder of the `EFC.AgGateway.Integration` project.

### Mapping

Any new request that needs mapping should have a new profile class created in the `Mapping/Profiles` directory that inherits from the `AutoMapper.Profile` class. Please see the `PriceSheetProfile` for an example.

The `AgGatewayMapping` class automatigally will apply the mapping configuration for any class inheriting from the `AutoMapper.Profile` class using Reflection.

### `IAgGatewayService` Interface

Whenever new functionality is added, add the method signature to the `IAgGatewayService` interface. For example:

```csharp
IEnumerable<ImportSeedItem> GetImportSeedItems(PriceSheetRequest priceSheetRequest, string endpoint = null);
```

This returns a DTO and accepts a DTO and an optional endpoint to append to the base url that exists on the `APMASTER` table.

### Operations Supplied From the [Standard `WSDLs`](https://oagi.org/DownloadsResources/ChemeStandards54/tabid/187/Default.aspx)

When coding the actual request within a method in the `AgGatewayService` class, it will be coded as follows, utilizing the [security provider](#security) to make the actual request:

```csharp
using (var client = new CESOrderManagementPortTypeClient(binding, endpointAddress))
{
    priceSheet = _agGatewaySecurityProvider.ExecuteRequest(client, request, client.PriceSheetRequest);
}
```

The `request` as an AgGateway request object that exists in the `EFC.AgGateway.Integration.Specifications.AgGatewayServiceContract` namespace.

The third parameter is the method on the client you wish to call.

The return object is a response object that exists in the `EFC.AgGateway.Integration.Specifications.AgGatewayServiceContract` namespace.

Please see the `AgGatewayService.GetImportSeedItems` method for an example in context.

### Operations Supplied From the [AgGateway DocExchange Generic `WSDL`](https://services.monsanto.com/AgGateway?WSDL)

**NOTE:** I found this URL in an old document from Monsanto. There's no real documentation, but it works.

When coding the actual request within a method in the `AgGatewayService` class, it will be coded as follows:

```csharp
var response = HandleGenericAgGatewayRequest<Generic.ExampleAgGatewayRequestType, Generic.ExampleAgGatewayResponseType>(request, endpoint);
```

The `request` as an AgGateway request object that exists in the `EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract` namespace.

The first type parameter will be a request object that exists in the `EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract` namespace.

The second type parameter will be a response object that exists in the `EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract` namespace.

Please see the commented out `ExampleGenericAgGatewayMethod` method in the `AgGatewayService` for an example i context.

## Directory Structure

### Exceptions

This contains the exceptions thrown by this library.

#### `AgGatewayProtocolException`

This is a custom exception that includes a `Fault` object, received when the service call returns a 400 series status code.

### Models

This is intended to contain any models used by this assembly.

#### Partials

This contains all partial classes used to extend functionality of the auto-generated classes. Please ensure the namespace of these classes matches the namespace of their auto-generated counterpart.

### Security

This contains all of the security providers used to provide credentials to each request made to an external AgGateway service. Each provider has a single method which acts as a wrapper around the request to provide the credentials in the appropriate manner. Any new security providers should implement the `IAgGatewaySecurity` interface.

### Specifications

This contains the auto-generated service/data contracts to communicate with any external AgGateway service, as well as all resources needed to generate said contracts. More information can be found in the `README.md` file within this folder.

### Utility

This contains various static utility classes utilized within this library.
