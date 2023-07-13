# The-Blog

The-Blog is a blog web application developed using ASP.NET Core, aimed at learning .NET development. This project utilizes various technologies, including Entity Framework Core, Microsoft SQL for the database, Redis for caching, Angular for the frontend, and JWT for authentication. The architecture of the project follows the principles of Clean Architecture and SOLID principles, emphasizing maintainability and extensibility.

## Features

-   **User Accounts**: The blog supports two types of accounts - normal and admin accounts. Normal accounts can leave comments under posts, while admin accounts have additional privileges.
-   **Admin Privileges**: Admin accounts have the ability to create posts, publish them, or save them as drafts. They can also edit or delete posts and delete commentaries.
-   **Admin Tools**: Admin accounts have access to admin tools, where they can assign admin rights to normal accounts. It is also possible to delete user accounts; however, the deletion of all admin accounts is restricted.
-   **Post Management**: Each post consists of a header, text, and optional tags. Posts can be filtered by tags by clicking on them.

## Technologies Used

-   **ASP.NET Core**: The web application framework used to build The-Blog.
-   **Entity Framework Core**: An object-relational mapper used for database operations.
-   **Microsoft SQL**: The chosen database for storing blog data.
-   **Redis**: Used for caching, enhancing performance by storing frequently accessed data.
-   **Angular**: The frontend framework utilized for building the user interface of the blog.
-   **JWT**: JSON Web Tokens are employed for authentication and authorization within the application.

## Project Organization

The project has been structured following the principles of Clean Architecture and SOLID principles. This organization promotes separation of concerns, testability, and maintainability. The codebase is divided into different layers, including:

-   **Presentation Layer**: This layer consists of the Angular frontend responsible for rendering the user interface.
-   **Application Layer**: The application layer handles the business logic and orchestrates the interaction between the presentation and domain layers.
-   **Domain Layer**: This layer contains the core business entities, such as posts, comments, and user accounts.
-   **Infrastructure Layer**: The infrastructure layer includes implementations of data access, caching, and external services. It also provides support for authentication and authorization.
