# MyFinance
## Technologies
### Frontend
1. Vue 3
2. Pinia for state management

### Backend
1. .NET 6
2. MoongoDb as main database (Unit Of Work and Repository patterns will be applied. MongoDb Atlas will be used for cloud database)
3. The project will be built following Clean Architecture's good practices, patterns and structure 
4. Mediatr (CQRS - Command Query Responsibility Segregation)



## Insights & lessons Learned
### Frontend

### Backend
- [Fluent Results + Mediatr Pipelines](https://code-maze.com/cqrs-mediatr-fluentvalidation/)
- The configuration ".EnableRetryOnFailure" is usefull when starting SQLServer connection running API and DB on Docker Containers. But using this will disable Transactions due to different execution strategies. [More here](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-resilient-entity-framework-core-sql-connections) 
- In this [Video](https://www.youtube.com/shorts/eXBX9ubqqJg) it is possible to see that the API Controller is not receiving the Mediatr's Query or Command, is receiving a DTO and the DTO is used to instantiate a Command or Query 