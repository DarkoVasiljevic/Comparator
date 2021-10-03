# Comparator API
Enables comparison of two strings and provide the result.

## Info
```bash
Api contains three endpoints:
  >>  Request URL <=> https:://localhost:44389/v1/diff/<ID>/left, 
      Method      <=> PUT,
      Usage       <=> Create 'left' string.
     
  >>  Method      <=> PUT,
      Request URL <=> https:://localhost:44389/v1/diff/<ID>/right,
      Usage       <=> Create 'right' string.
      
  >>  Method      <=> GET,
      Request URL <=> https:://localhost:44389/v1/diff/<ID>,
      Usage       <=> Compare two strings and return result
      
