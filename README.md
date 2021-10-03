# Comparator API
Enables comparison of two strings and provide the result.

## Info
```bash
Api contains three endpoints:

  >>  Request URL <=> PUT,
      Method      <=> https:://localhost:44389/v1/diff/<ID>/left, 
      Usage       <=> Creates a 'left' string with <ID> if it does not exist or updates it if it exists,
                      On success = Satatus Code : 201 Created,
                      On error   = Satatus Code : 400 Bad Request.
     
  >>  Method      <=> PUT,
      Request URL <=> https:://localhost:44389/v1/diff/<ID>/right,
      Usage       <=> Creates a 'right' string with <ID> if it does not exist or updates it if it exists,
                      On success = Status Code : 201 Created, 
                      On error   = Status Code : 400 Bad Request.
      
  >>  Method      <=> GET,
      Request URL <=> https:://localhost:44389/v1/diff/<ID>,
      Usage       <=> Compare two strings and return result:
                      If one of the strings with <ID> does not exist return Response: 404 Not Found,
                      If both strings exists returns:
                      On success = Status Code : 200 OK , 
                                   Response body : { "resultType": "Equals", "diffs": [] },
                                || Status Code : 200 OK , 
                                   Response body : { "resultType": "SizeDoNotEqual", "diffs": [] },
                                || Status Code : 200 OK , 
                                   Response body : { "resultType": "ContentDoNotEqual", "diffs": [{ "offset": 0, "length": 2 }] },
                      On error   = Status Code : 404 Not Found.
