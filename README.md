# Smarter Sleep Prototype
Smarter Sleep is an smartphone application that automates your sleep and wake routines
by managing smart home devices to improve sleep quality. For more information about the prototype design access the Smarter Sleep website below:

**[Current Website](https://www.cs.odu.edu/~411blue/)** • [Archived Link](https://www.cs.odu.edu/~cpi/old/411/bluef23/)<br>*The Current Website points towards the current Team Blue 411W project, at some unknown date the Smarter Sleep website will then be archived to the second address.*

## Installing
As the project is a prototype, there are no official releases and the project must be compiled to execute. The structure of the code is divided into four sections which run entirely seperated, for more information go to the associated README file.
- ASP.NET RESTful Api • **[Source Code](Backend/WebApi)** • [README](Backend/WebApi/README.md)
  - Hosted Remotely on an AWS EC2 Instance
- PostgreSQL Database • **[Source Code](Backend/initial_database_setup.sql)**
  - Stored on an AWS RDS PostgreSQL Database
- Node.js Lambda Script • **[Source Code](Backend/LambdaFunctions/UserRegistration)** • [README](Backend/LambdaFunctions/UserRegistration/README.md)
  - Connected to a `Post Confirmation Lambda Trigger` attached to a AWS Cognito User Pool.
- Flutter Application • **[Source Code](Frontend/SmarterSleep/smarter_sleep)** • [README](Frontend/README.md)
  - Executed on physical or emulated mobile devices.

## Team Blue Fall 2023
Team Positions
- [Jake Leith - Team Lead | Webmaster | Backend Developer](https://github.com/JL-SoftDev)
- [William Tonkinson - Full Stack Developer | Database Specialist](https://github.com/wtonk001)
- [Jozef Newsome - Frontend Developer](https://github.com/JMNewsome)
- [Russell Mack - Frontend Developer](https://github.com/rmack1020)
- [Scott Mcallister - Frontend Developer](https://github.com/RedstoneRm)

Instructor
 - Professor J. Brunelle - [Old Dominion University](https://www.odu.edu/)

