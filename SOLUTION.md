# AVIV technical test solution

You can use this file to write down your assumptions and list the missing features or technical revamp that should
be achieved with your implementation.

I spent approximately 1 hour completing this test.

## Notes

Write here notes about your implementation choices and assumptions.

## Questions

This section contains additional questions your expected to answer before the debrief interview.

- **What is missing with your implementation to go to production?**
  Implementing caching mechanism and using load balancing to distribute traffic. 
  Implementing authentication and authorization mechanisms.
  Stroring configs in git while securing the passwords by encryption or put the config in DB.

- **How would you deploy your implementation?**
  I consider three branches for each service.
  1-Dev 2-Stage 3-Production
  when a new feature is under development we create a branch from Dev and after implementation 
  it is merged with Dev then Dev branch is merged with Stage to be tested by product team and for production 
  it is merged with Production branch. The update is done by running the docker file that can be used in a CI-CD process.
  If there is a CI/CD process so it is configured in git that when we push on the production branch 
  It automatically updates the production by running the configured scripts such as docker file and DB scripts.
  
- **If you had to implement the same application from scratch, what would you do differently?**
  I would use Clean Architucture and MediatR and CQRS patterns, and I would use APIGateways.
  
- **The application aims at storing hundreds of thousands listings and millions of prices, and be accessed by millions
  of users every month. What should be anticipated and done to handle it?**
  
  Caching mechanisms: we can use in memory caching so each service can respond to requests. Additionally using a distributed caching like redis makes our 
     caching mechanism appropriate for scalability. Therefore if instance A updates cache and the same request is processing in instance B the cached
     response in redis will be used.
  
  Load balancing: we can use a load balancer such as NGINX to distribute traffic across multiple instances of our application and ensure high availability and
     scability. In this case we consider making services stateless to make it easy to have multiple instances of a service type.
     
  Scaling: we can use a container orchestration tool such as Kubernetes to automatically scale our application based on the current load and ensure that 
      it can handle spikes in traffic.In this case the architecture is flexible to employ elastic strategies.
  
  NB : You can update the [given architecture schema](./schemas/Aviv_Technical_Test_Architecture.drawio) by importing it
  on [diagrams.net](https://app.diagrams.net/) 
