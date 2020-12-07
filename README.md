## Sympli Coding Challenge

Fetch google ranking of an URL with given keywords

### How to run?

This project uses google api to grab the search result.
You will need to setup a programmable search engine (on google) and obtain a free project api key.
However, I have one that I use for development. However, these free api key has 1000 requests limit per day so It could run out of quota pretty fast depending on how many searches is requested.

To run the server, navigate to the server folder:

run

```
key=YOUR_API_KEY cx=YOUR_SEARCH_ENGINE_ID dotnet run
```

or (to use my key)

```
key=AIzaSyAuZ3As93VfnWruz9iqKbV1ZTz_LOsnLT4 cx=9ac87101b9f6dfb4c dotnet run
```

It should host on port 5001

to run the client, navigate to the client folder:

```
yarn run start
```

It will start running at localhost:3000

### Comments

I've ran out of time for extension 2.
However should I have the time for it; Extension 2 would require another implementation of ISearchEngine for Bing
and potential slight modification to the json response object to cater for the different types of search engine.

I've also made a mistake of using Google Search API which I've only realized later that this was not allowed as part of
the requirements. However, I ran out of time to redo or make the change to a web scraping service.

In terms of performance, availabilty and reliablity; it would be ideal to firstly have this service publishes metrics to a metric service like
datadog where you could monitor how its being used. We would want to monitor its cache hit rates, the amount of requests its getting, time taken to compute the ranking and the time taken to fire and gather requests/responses from the respecting 3rd party search engines. From there, we could tune accordingly.

For example, for redundancy and better throughput one could potentially have multiple instances of this service running in production.
And so, it might be beneficial to pull out the in memory caching and replace them with a shared memory data store like Redis/Elasticache to improve cache efficiency. Not only this will improve the effectiveness of cache across multiple services; it will also reduce the risk of memory usage blowing up on the services which could potentially crash the service due to OOM.
