# Quick Start
1. Install docker
2. docker compose up -d
3. Api is available at `http://localhost:7101/api` or `https://localhost:5290/api`
4. You can test the api in Insomnia with [ProductApi-insomnia.json](../Insomnia/ProductApi-insomnia.json)


# Architecture

## Overall Approach
I separated logic into controllers and services. Controllers handle route parameters and return proper status codes along with errors and data. Expected errors are handled in the controller, but unexpected ones will bubble up to the global error handler. The services handle the bulk of the logic. They interact with the database to do the work that was requested, then return the result to the controller.

Each DB Table is represented by a Model with a property for each of it's columns. The model represents a row in the database and also takes advantage of EF Core's Relationships to allow querying for related data. 

I used DTOs in order to limit the data being sent to/from the API. e.g. CategoryCreateDto only has Name and Description because Id and IsActive cannot be set by the User. ProductSummaryDto leaves out the Category relationship so that it does not return circular data.

## Database Schema
I made assumptions of the types from the property names. If I had more time I would have put more indexes in, but I don't have a lot of SQL experience, and I didn't want to include code that I couldn't explain thoroughly.
With more time I would have added an index for IsActive on both tables because it is used by nearly all of the GET endpoints.

## Technology Choices

I chose to use the Minimal Api pattern with Dotnet because this project is not complex and the dotnet docs recommend it for new projects that don't have a need for features of MVC.

The database is Postgresql. I chose that because I have some experience with it and I know it is pretty much the standard for free SQL databases.

All services run in docker using docker compose. This allows you to run everything with a single command and not need to install any
dependencies. This isn't complex enough to require Kubernetes.

# Design Decisions

I Applied the SRP by splitting my code between Program, Controllers, Services, Models, and Migrations, and by splitting Category and Product. 

I



