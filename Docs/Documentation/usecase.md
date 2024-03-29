# Use Case: Anonymous User Creates an Account on the Platform

**ID:** UC1 - Account Creation

**In order** to personalize my experience and build an online identity  
**As an** anonymous user  
**I want to** create an account on the platform

**Note:** This test assumes that the system generates unique IDs for users internally and that the username, first name, last name, and email are provided by the user.

## Success Scenarios

| ID  | Given                                           | When                                    | And                                         | Then                                                     |
|-----|-------------------------------------------------|-----------------------------------------|---------------------------------------------|----------------------------------------------------------|
| S3  | An anonymous user is on the account creation page | They enter "ValidUsername1" as the username | The username is unique and has valid characters | The system creates the account and sends a verification email |
|     |                                                 | They enter "Jane" as the first name     |                                             |                                                          |
|     |                                                 | They enter "Doe" as the last name       |                                             |                                                          |
|     |                                                 | They enter "email@example.com" as the email |                                             |                                                          |
| S5  | An anonymous user is on the account creation page | They enter "CreativeName" as the username | The username is unique and has valid characters | The system creates the account and sends a verification email |
|     |                                                 | They enter "Alice" as the first name    |                                             |                                                          |
|     |                                                 | They enter "Johnson" as the last name   |                                             |                                                          |
|     |                                                 | They enter "alice.johnson@domain.com" as the email |                                             |                                                          |

| ID  | Given                                           | When                                    | And                                         | Then                                                     |
|-----|-------------------------------------------------|-----------------------------------------|---------------------------------------------|----------------------------------------------------------|
| S4  | An anonymous user is on the account creation page | They enter "JaneDoeX" as the username   | The username is unique and has valid characters | The system creates the account and sends a verification email |
|     |                                                 | They enter "Jane" as the first name     |                                             |                                                          |
|     |                                                 | They enter "Smith" as the last name     |                                             |                                                          |
|     |                                                 | They enter "jane.smith@example.co.uk" as the email |                                             |                                                          |
| S6  | An anonymous user is on the account creation page | They enter "FutureLeader" as the username | The username is unique and has valid characters | The system creates the account and sends a verification email |
|     |                                                 | They enter "Charlie" as the first name  |                                             |                                                          |
|     |                                                 | They enter "Brown" as the last name     |                                             |                                                          |
|     |                                                 | They enter "charlie.brown@anotherdomain.com" as the email |                                             |                                                          |
