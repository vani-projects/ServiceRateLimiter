# Rate Limiter Service
Implemented a Rate Limiter service that can be accessed over an API endpoint. The Rate Limiter service limits the number of SMS calls that are allowed per phone number and also total number allowed for the account.

I have provided 2 different implementations for this service. One implementation uses an in-memory dictionary to keep track of the number of calls/requests that are made for specific phone numbers. The other implementation uses redis cache to keep track of the count. Both the services will clean up resources if we do not get any service requests for phone numbers after a certain inactive time.
