# Quick Start
1. Install docker
2. docker compose up -d
3. Api is available at `https://localhost:5290/api`
4. You can test the api in Insomnia with [ProductApi-insomnia.json](../Insomnia/ProductApi-insomnia.json)
5. Within `product-client/` run `npm i` then `npm start` and navigate to `http://localhost:4200`

# Architecture

## Overall Approach
I separated logic into controllers and services. Controllers handle route parameters and return proper status codes along with errors and data. Expected errors are handled in the controller, but unexpected ones will bubble up to the global error handler. The services handle the bulk of the logic. They interact with the database to do the work that was requested, then return the result to the controller.

Each DB Table is represented by a Model with a property for each of it's columns. The model represents a row in the database and also takes advantage of EF Core's Relationships to allow querying for related data. 

I used DTOs in order to limit the data being sent to/from the API. e.g. CategoryCreateDto only has Name and Description because Id and IsActive cannot be set by the User. ProductSummaryDto leaves out the Category relationship so that it does not return circular data.

## Database Schema
I made assumptions of the types from the property names. If I had more time I would have put more indexes in, but I don't have a lot of SQL experience, and I didn't want to include code that I couldn't explain thoroughly.

## Technology Choices

I chose to use the Minimal Api pattern with Dotnet because this project is not complex and the dotnet docs recommend it for new projects that don't have a need for features of MVC.

The database is Postgresql. I chose that because I have some experience with it and I know it is pretty much the standard for free SQL databases.

All services run in docker using docker compose. This allows you to run everything with a single command and not need to install any
dependencies. This isn't complex enough to require Kubernetes.

I used Copilot through VS Code to help out.

I used DBeaver to interact with the database directly during development.

# Design Decisions

I applied the SRP by splitting my code between Program, Controllers, Services, Models, and Migrations, and by splitting Category and Product. 

I applied Dependency Inversion and Dependency Injection by using defining interfaces for the services and adding them to the DI container. The controller only knows about the interface, and in the future, I could swap the implementation for the controller by only changing
`builder.Services.AddScoped<ICategoryService, CategoryService>();` to reference a different implementation.

I chose to do the search endpoint because I wasn't as comfortable with writing an efficient aggregation query. The search specification is only concocting a complex where clause after parsing the user input.

I chose not to implement the repository pattern because of the small scale of this project. It doesn't require me to be able to interact with different data sources, or swap dev/production/in memory databases. Testing is simple because I just saved the endpoints in insomnia, and when I want to start over, I just run the migrations up and down again.

The indexes are the primary ids, and the foreign key is CategoryId. I put indexes on IsActive for both tables because it is frequently used in queries. With more time and usage data I would add indexes to some or all of the columns in product, depending on how users used the Search endpoint.

# What I Would Do with More Time

1. Improve the error handling to be more uniform.
2. See if there is a way to use the Minimal API without needing to inject the same service in each method.
3. Typed responses in the API
4. Optional features from the front end.
5. Reorganize directory (ProductApi is a confusing name for a project with an angular client inside)

Production considerations:
1. Move db secrets into env vars.
2. Add HTTPS
3. Add indexes for most used columns in the search endpoint.
4. Documentation with OpenApi
5. Share DTOs between front and back end
6. Serve client through Nginx


# Assumptions and Tradeoffs

## Assumptions
The main assumption is the scale of this project. There are things I could have done that would make sense in a production environment that would have just taken too long for this assignment. I could have added Nginx between the web ui and the api, I could have run it in Kubernetes, and I could have included unit tests that swapped the real DB for an in memory one.

## Tradeoffs
I chose Minimal API instead of MVC because it is simpler for smaller scale projects. I chose to use Insomnia to test the API instead of including automated tests because it is faster for the small scale, and because there isn't a lot of logic to test which isn't easily tested just by hitting the API.
I chose to use DbContext directly instead of creating a Repository, again because of the small scale and time constraints, and the fact that I wasn't using unit testing.


