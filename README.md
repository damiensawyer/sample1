Hi, 

Attached is my repsonse to your code quiz. 

I've done three versions, moving from V1 as the most straightforward, then adding complexity. 

My thinking behind this is that different teams appreciate different approaches to problem solving. Sometimes a more sophisticated solution might be more trouble than required. Also, with increased complexity, so too increases the workload with regards to team training, hiring etc. 

V1 provides a simple solution with POCOs, simple tests etc. 
V2 adds interfaces, benchmarking and more advanced XUnit tests. 
V3 extends the patterns in V2 to provide a 'test wrapper' which allows for async setup for a test. The idea being that you could write something like a Redis / Postgres implementation which requried setup, but still utilize the same interfaces and tests. 

There's a lot which I left out of these samples. No IoC etc. There's also no mocking - which I've moved away from in recent years. If I were to extend this Xunit.MemberData pattern further, I might do somethign like incorporate IoC into the classes which are fed to MemberData. The point being that we should be able to easily add an implementation of the the interface with minium alternation to the tests. 

Thanks very much for this opportunity. 

Damien