# AWS Lambda Function for Adding Cognito Users to a PostgreSQL Database

This AWS Lambda function is designed to be triggered when a user signs up through Amazon Cognito. It will add the user's information to a PostgreSQL database.

## Prerequisites

Before you can deploy and use this Lambda function, make sure you have the following prerequisites in place:

1. **AWS Account:** You should have an AWS account set up.

2. **Amazon Cognito:** Set up and configure Amazon Cognito to handle user sign-ups. Obtain the necessary Cognito triggers and event data.

3. **Amazon RDS (PostgreSQL):** You should have a PostgreSQL database hosted on Amazon RDS.

4. **Node.js:** You'll need Node.js installed.

## Installation

1. Clone this repository to your local development environment.

2. Install the required Node.js modules using npm:

    ```bash
    npm install
    ```

3. Set up your AWS Lambda function:

   - Create a new Lambda function in the AWS Management Console.
   - Upload the code by creating a ZIP deployment.

4. Configure environment variables:

   - Set the `DB_CONNECTION` environment variable to the connection string of your PostgreSQL database.
   
5. Configure your Lambda function trigger:

   - In the AWS Lambda console, configure the trigger for your function to be triggered by Amazon Cognito when a user signs up.

## Usage

This Lambda function will be triggered automatically when a user signs up through Amazon Cognito. It will add the user's information to your PostgreSQL database.

## Database Table

This Lambda function inserts the user's information into a PostgreSQL table named `app_user`. Ensure that the table structure matches the function's SQL query. The sample query in the code adds the following fields:

- `user_id`: The unique identifier for the user.
- `username`: The username provided by the user during sign-up.
- `created_at`: The timestamp when the user signed up.
- `points`: An initial point value (set to 100 in the sample code).

## Handling Errors

This function logs any errors encountered during the database insert operation. Make sure to monitor the AWS CloudWatch logs for any issues.

## Author

William Tonkinson

