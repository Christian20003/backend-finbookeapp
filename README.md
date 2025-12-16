# FinBooKeApp - Backend
This repository includes the backend of the financial-book-keeping application. The frontend is under the following link: https://github.com/Christian20003/financial-book-keeping-app. 

<big>IMPORTANT:</big> This application is still in development and not ready for production.

# Goal
This application was designed to help individual users manage their finances. By recording income and expenditure and assigning them to different categories and payment methods, the aim is to make it as easy as possible for users to manage their household finances.

# Instructions to run the backend 

1. Make sure you have installed the following tools:
    - dotnet (version 8)
    - docker
2. Clone this repository and and change to the directory:
    ```console
    cd ./backend-finbookeapp
    ```
3. Fill up the **appsettings.json** file or use **dotnet user-secrets**. Each key that has an emtpy string as value needs a valid input. An explanation for each key is described in section [appsettings](#appsettings)
4. Create a **.env** file which must include the following data:
    - DATABASE_USERNAME: \<your username\>
    - DATABASE_PASSWORD: \<your password\>
    - MONGO_REPLICA_SET_KEY: \<your secret in base64\>
    
    Ensure that the password and username correspond with the values in the **appsettings.json** file or the **dotnet user-secrets**
5. Start the database with the following command:
    ```console
    docker compose up -d
    ```
6. Start the server:
    ```console
    dotnet run
    ```

The server should now be running and listening on port 5038. You can access the Swagger documentation at the URL http://localhost:5038/swagger/index.html.

# Appsettings

1. **AuthDatabase:** Settings for the database that stores authentication data
    - ConnectionString: The connection string is what enables connection to the MongoDB database. If you do not know how this works, see https://www.mongodb.com/docs/manual/reference/connection-string/
    - DatabaseName: The name of the database where all the data should be stored.

2. **FinancialDataDatabase:** Settings for the database that stores everything else.
    - ConnectionString: The connection string is what enables connection to the MongoDB database. If you do not know how this works, see https://www.mongodb.com/docs/manual/reference/connection-string/
    - DatabaseName: The name of the database where all the data should be stored.

3. **JwtConfig:** This application uses Java-Web-Tokens for authentication.
    - Audience: The URL of the server which receives that token.
    - Issuer: The URL of the server which created that token.
    - AccessTokenSecret: A secret to generate a symmetric key for signatures on access tokens.
    - RefreshTokenSecret: A secret to generate a symmetric key for signatures on refresh tokens.
    - AccessTokenExpireM: How long should an access token be valid in minutes.
    - RefreshTokenExpireD: How long should a refresh token be valid in days.

4. **Smtp:** This application must send emails to users in specific scenarios, so an SMTP server must be reachable.
    - Host: The host name.
    - Port: The port on which the SMTP-Server is listening.
    - Username: The username for authentication.
    - Password: The password for authentication.
    - Address: The sender's email address.