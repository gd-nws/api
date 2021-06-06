# Good News! API

.Net Core implementation of the Good News API.
The API handles:
  - Fetching headlines.
  - Searching headlines.
  - User sessions (always anonymous).
  - Headline annotations.

## Documentation

Docs can be found at:
  - [localhost](http://localhost:5000)
  - [Good News API](https://api.gdnws.co.uk)
  
## Running Locally

Copy the `.env.example` file to `.env`, and update with your database details.
Then run: `cd ./src/GoodNews && dotnet restore && dotnet run`.

It can be ran in watch mode to respond to file changes with:
`dotnet watch run`.