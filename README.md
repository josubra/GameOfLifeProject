## GameOfLifeProject

A .NET Core API implementation of Conway's Game of Life.
More about the game: <https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life>

# Assumptions Made

Based on the functional requirements:

## Assumptions & Implementation Details

Based on the functional requirements, the API provides the following capabilities:

1. **Upload a new board state and return its ID**  
   - A `POST` endpoint accepts a board configuration save it and returns a `boardId` for future reference.

2. **Retrieve the next state of a board**  
   - A `GET` endpoint accepts a `boardId`, computes the next state based on Game of Life rules, save it and returns the updated board.

3. **Retrieve a board's state after a given number of steps**  
   - A `GET` endpoint accepts `boardId` and `steps` as parameters, calculates the board's state after the given steps, save it and returns the result.

4. **Retrieve the final stable state of a board**  
   - A `GET` endpoint accepts `boardId` and `maxAttempts`. It simulates the board's evolution until it reaches a stable state (i.e., no changes between steps). If a stable state is found within `maxAttempts`, the state is saved and returned; otherwise, an error (`Not Found`) is returned.

5. **Retrieve the current board state by ID** *(Additional Feature)*  
   - A `GET` endpoint returns the current board configuration based on `boardId`.

### Additional Assumptions

- Tests for the Web API were not initially implemented but can be added easily.
- The board state history is not stored in the database, as it was deemed unnecessary. However, this can be implemented if needed.

---

## Dependencies

### Environment
- Docker Desktop: <https://www.docker.com/products/docker-desktop/>  
  _(Required for running API, Database, and Tests in containers)_

### Project Stack
- **.NET Core 7.0**

### NuGet Packages
#### API
- `Microsoft.EntityFrameworkCore 7.0.20`
- `Microsoft.EntityFrameworkCore.Design 7.0.20`
- `Swashbuckle.AspNetCore 6.5.0`
- `Microsoft.EntityFrameworkCore.Tools 7.0.20`
- `Microsoft.VisualStudio.Azure.Containers.Tools.Targets 1.21.0`
- `Pomelo.EntityFrameworkCore.MySql 7.0.0`

#### Tests
- `Microsoft.EntityFrameworkCore 7.0.20`
- `Microsoft.EntityFrameworkCore.Design 7.0.20`
- `Swashbuckle.AspNetCore 6.5.0`
- `Microsoft.EntityFrameworkCore.Tools 7.0.20`
- `Microsoft.VisualStudio.Azure.Containers.Tools.Targets 1.21.0`
- `Pomelo.EntityFrameworkCore.MySql 7.0.0`

---

## Setup Instructions

1. Clone the repository.
2. Open the solution in **Visual Studio 2022 or later**.
3. Build the project and ensure all NuGet dependencies are resolved.
4. Open **Docker Desktop**.
5. Set **Docker Compose** as the startup project and run it.
6. Ensure that two containers start:
   - One for the API.
   - One for the MySQL database.
7. **Swagger** is pre-configured for the development environment to facilitate API testing.

---

## API Documentation

Refer to `GameOfLifeAPIDocumentation.json` for detailed API specifications.

---

# API Endpoints

Check the file GameOfLifeAPIDocumentation.json

## Scalability Considerations

- The API leverages a **MySQL database** to store board data, allowing for potential historical state tracking if required.
- **Containerization with Docker** ensures ease of deployment and scalability. The API can be scaled **horizontally** using container orchestration tools like Kubernetes or Docker Swarm.
- **Load Balancing & Auto-Scaling:**
  - If the API experiences high traffic (e.g., thousands of requests per day), it can be deployed behind a **load balancer** (such as Nginx or AWS ALB) to distribute requests efficiently.
  - **Horizontal scaling** can be achieved based on key performance metrics (e.g., CPU usage, requests per second) by deploying additional instances.
  - A **caching layer** (e.g., Redis) can be introduced to optimize repeated board state queries and improve response times.
- **Database Optimization:**
  - Indexing strategies can be applied to frequently queried fields to enhance performance.
  - Read-replica databases can be introduced to distribute database load in read-heavy scenarios.

---

## Additional Notes

- Ensure API containers are **not running in the background** while executing tests. The database containers for both API and tests use the same port, which may cause conflicts.
