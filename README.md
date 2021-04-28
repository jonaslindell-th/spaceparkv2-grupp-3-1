# SpacePark MVP 1

![SpacePark](spacepark.jpg)

For the procedures used in this project see : [Procedures](Procedures.md)

For the documentation by the team see: [Documentation/readme.md](Documentation/readme.md)

# SpacePark

When traveling across space you once in a while need to take a break, and park you spaceship, you do that at a spaceport. The thing is just that spaceports are just like any parking lot on earth controlled by an evil parking company, the biggest of these is SpacePark (this company)!

The company SpacePark, owns several spaceports accrose the universe.

## Your assignment

You have already been part of the the team which developed the first protoype (a console application) of this application, and the management have seen the potential of the application, and wish to take the next step in making your prototype into a product. The mangement find that making the application available as a web application is the most natural step, and they wish to follow an [API first stategy](https://swagger.io/resources/articles/adopting-an-api-first-approach/).

The goal of your team at SpacePark is still to develop an application which register parkings and close the sparceport when it's full (and open when there is room, and only for spaceships which fits in). All parkings should be registered in a database, which is created using Entity Framework Core and code first. All queries to the database should be done using Entity Frameworks fluent API. The new twist to this is that all the logic should be contained within a REST API, and only this API should be able to access the database.

As the logic will now be collected in one web API should it now be a [multi-tenant](https://relevant.software/blog/multi-tenant-architecture/) application, which means that the API should have support for multiple spaceports. 

The mangement in SparkPark is at the moment considering if this system in the future should be sold as a product to other spaceport-operators, but that have not been decided yet.

### Exclusive spaceport
This is still though to be an **exclusive** spaceport and ONLY famous space travelers which have been part of a Starwars movie can use the spaceport. The travlers should identify them self when arriving, be able to pay before they can leave the parking lot and get an invoice in the end. 

You can test if the user has been part of Starwars by using the Starwars Web API: [swapi.dev](https://swapi.dev/), you are not allowed to cache the data from the API, which means that you will need to request the API each time you need to fetch data. These requests should be made asynchronously.

A recommendation (but no requirement) is to use the Nuget package [RestSharp](https://restsharp.dev/). You will need to implement the classes to be used by the build-in ORM in RestSharp.

```c#
var client = new RestClient("https://swapi.dev/api/");
var request = new RestRequest("people/", DataFormat.Json);
// NOTE: The Swreponse is a custom class which represents the data returned by the API, RestClient have buildin ORM which maps the data from the reponse into a given type of object
var peopleResponse = await client.GetAsync<Swreponse>(request);

Console.WriteLine(peopleResponse.Data.Count);
foreach (var p in peopleResponse.Data.Results)
{
    Console.WriteLine(p.Name);
}
```

The travlers only use starships which have been part of a Starwars movie (see the endpoint /starships). They should be able to select their starship on arrival in the application.

All request to the Starwars API should be made asynchronous. 

### Users

There is two kind of users in this application.

The vistors, which can:
* Register a parking
* Get information on current parking
* End parking and "pay"
* Get information on all previous parkings

The administrator, which can:
* Add new spaceports to the system

## Your code

Develop a REST API using ASP.NET Core.

It should be possible to perform all opertions through requests to the API, if you would like to develop an interface to this application should it be a console application which makes requests to the API.

The application should save the parkings, payments etc to the a database defined using [Entity Framework](https://docs.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli) och code first.


### Access validation

Along with all requests in the system should be passed a HTTP header with an API key it could look like:
```http
GET /spaceports HTTP/1.1
apikey: secret1234
```

Evaluate this key with a custom [middleware](https://pgbsnh20.github.io/PGBSNH20-backendweb/lectures/middleware) which reads the `apikey`-header and evaluates if the key is approved, if not should the request be rejected.

It not a requirement to have different keys for different users, nor is it a requirement that the key should provide access different functionality. We trust our users, so that do not missuse the system.

## Database

The database used in the project should be defined in a docker-compose file, so that all developers can use the same connection string in the project.

## Given

Some files are given in this repository.

**Documentation**

The folder initially only contains one file called *readme.md*, this file is more or less empty.

In this folder should you place digital representations of all documentation you do. Swagger, screenshots, photos (of CRC cards, mindmaps, diagrams).

Please make a link and descriptive text in the *readme.md* using the [markdown](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet) notation:

```markdown
# Our spacepark project
What we have done can be explained by this mindmap.
![Mindmap of spaceport](mindmap.jpg)
Bla bla bla bla
```

An API can be described using the OpenAPI notation, and ASP.NET have by default the Swashbuckle UI enabled in development mode. Include screenshots from Swashbuckle to document your API structure.

Remeber to describe how to run and configure the appliction, and which ports and endpoints is available.

**Source**

The source folder is more or less empty, it just contains one file a Visual Studio solution file, called SpacePark.sln.

New code projects should be added to this file.

# Getting started

These are **suggestions** on how you can get started on this project:

1. Discuss the assignment (and document this in the *documentation*-file), so you have a common picture of what you are building, especially which endpoints and methods the API support
1. Open the given solution-file in Visual Studio and add a new web api:
   * Use the template: ASP.NET Core Web Application
   * Choose a project name
   * Select: API  
1. Start implementation of the API
1. Implement a **very simple** flow of a scenario and unit tests which can confirm that it works.

# Evaluation
This project will be evaluated on the following criterias:
* Code and structure 
* Unit tests in project
* Structure and documentation of API requests
* Presentation of the project in the end

# Suggestion
Use the tool REST Client for Visual Studio to test the API endpoint: [REST Client](https://github.com/Huachao/vscode-restclient), a nice effect of this tool is that your requests can be placed in your git-repo, and in that way can everybody in the team easily see examples of how to use the api.

You an also test the API using the swagger page, or a tool like postman.
