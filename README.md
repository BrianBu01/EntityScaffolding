# EntityScaffolding

Convention base Entity Framework Core Scaffolding

## Warning

This library uses public "INTERNAL" components of EFCore.  This means it will not have the long term support and may not be able to upgrade without removing these features.

> This is an internal API that supports the Entity Framework Core infrastructure and not subject to the same compatibility standards as public APIs. It may be changed or removed without notice in any release. You should only use it directly in your code with extreme caution and knowing that doing so can result in application failures when updating to a new Entity Framework Core release.

I have thought about how to generate the same behavior without these internal APIs.  It is possible, but we can cross that road if we ever have to.

## Features

- Overrides EFCore's Entity Generation
- By just applying the `[EntityConvention]` attribute to an interface, any entity generated that matches that definition will automatically have that interface applied.
- Extensible Points for creating custom conventions and custom entity editors.
- Ability to apply Attributes to Entity Properties.

### DefaultConventions Features

- IIdentity<T1 - Tx> will automatically be applied to entities with Primary Keys
- DbSet.FindEntity that is hard typed to IIdentity Generics
  - For Example: if your entity has a composite primary key of <int, int> will require a matching key DbSet.FindEntity(int, int)
- `[PrimaryKey(index)]` will be applied to Primary Key Properties for Entities.
- Some basic conventions to demonstrate usages of the library.
  - ISoftDelete
  - IInsertTracker
  - IUpdateTracker
  - IRowguid

## Usage

There are a couple steps to properly setup usage of this package.

### Adding EFCore

The required packages are :

- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Tools

### Adding Design Time Services

With in the assembly you are generating your Entities with EFCore Add the following Code.

```CSharp
using EntityScaffolding;
using EntityScaffolding.DefaultConventions;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace <YourNameSpace>
{
    public class DesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(new UseDefaultConventions());
            serviceCollection.AddSingleton<ICSharpEntityTypeGenerator, ConventionEntityTypeGenerator>();
        }
    }
}
```

If you do not wish to use the default conventions remove the using and AddSingleton call.

### Resolving Internals Interface ICSharpEntityTypeGenerator 

The `ICSharpEntityTypeGenerator` interface will most likely not be resolved by default, even though you have added the proper package.

The only solution I have currently figured out is to manually add the `Microsoft.EntityFrameworkCore.Design.dll` as a reference.

The location for this assembly will most likely be C:\Users\[UserName]\.nuget\packages\microsoft.entityframeworkcore.design\[version]\lib

### Applying Convention

Now when you run Scaffold-DbContext the EntityScaffolding library will be loaded and apply conventions to your entities.  Out of the box running this against Adventure Works will show conventions being applied.

### Custom Conventions

There are a few types of conventions built internally, but there are extension points to add more.

Note: What ever library that is storing your conventions, must be loaded during the DesignTimeServices.

#### EntityConvention Attribute For Interfaces

By applying a `[EntityConvention]` attribute to an interface, that interface will automatically be applied to any entity that matches the interface definition.

```CSharp
    [EntityConvention]
    public interface IPerson
    {
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
    }
```

Then the interface will automatically be append the interface to the Entity such as during Scaffolding:

```CSharp
    public partial class Person: IPerson
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
		
        //Additional Properties
    }
	
```

#### IInterfaceConventionMatcher

By implementing the `IInterfaceConventionMatcher` interface you can parse on your own, the `EntityType` object and determine if it is applicable to accept an interface.  The interfaces returned will automatically be applied.

```CSharp
    public class CustomInterfaceConventionMatcher : IInterfaceConventionMatcher
    {

        public IEnumerable<InterfaceElement> GetMatchingElements(IEntityType entityType)
        {
            
        }
    }
```

This although does not fill in any missing members. So make sure it will either match, or you will do it with partials.

#### IPropertyAttributeConventionMatcher

By implementing the `IPropertyAttributeConventionMatcher` interface you can apply Attributes to Properties of an Entity.

```CSharp
    public class CustomPropertyAttributeConventionMatcher : IPropertyAttributeConventionMatcher
    {
        public IEnumerable<PropertyAttributeElement> GetMatchingElements(IEntityType entityType)
        {
            
        }
    }
```

#### Custom Writable Elements

By creating a `IWritableElement` and implementing `IConventionMatcher<TWritableElement>`, you can add custom data to be processed by the Entity Editors.

Here you would again parse the EntityType and provide elements respective to the type.

#### Custom Editors

By implementing `EntityEditor<TWritableElement>` you can specify how to write the Elements with in the Entity Source Code.

You must implement the `EditEntityBase(string entitySource)` method.  The typical pattern is Define Insertion Point -> Append Contention -> `return entitySource.Replace(insertionPoint, insertionPointAndAppendedContent)`

Once you have your custom Editor, you can extend the ConventionConfiguration class and modify the EntityEditors.  Additional editors should be applied to the end of the existing list.  Then you must update `serviceCollection.AddSingleton` to use your new configuration.

## Building Solution


### Resolving EFCore Internals

The EFCore "INTERNALS" will most likely not be resolved by default, even though you have added the proper package.

There are two solutions to this problem:
1. Update the project file to exclude the additional elements under the package reference.  It should read

	```<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="x" />```
	
2. The location for this assembly will most likely be C:\Users\[UserName]\.nuget\packages\microsoft.entityframeworkcore.design\[version]\lib

