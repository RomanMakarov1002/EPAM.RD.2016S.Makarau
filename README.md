# EPAM.RD.2016S.Makarau

3. Task:

* Create a user storage system that allows to store users entities.

* A user is an entity with first/last name, date of birth, personal id (like in passport), gender (enum), an array of visa records (struct { country, start, end }). Add functionality to compare users, and get a hashcode of the entity.

* There should be several ways to store users (for ex. In DB), but we need only one implementation – in memory. But there should be a possibility to add an another implementation.

* Methods for storage:

o Add a new user: Add(User user) -> returns User ID

o Search for an user: SearchForUser(ISearchCriteria criteria) -> returns User IDs. At least 3 criteria.

* Possble to use predicate here SearchForUser(Func<T>[] criteria).

o Delete an user: void Delete(...)

* When creating a new there should be a possibility to change the strategy to generate an ID.

* When adding a new user there should be a way to set a different set of rules for validating an user entity before adding it: Add(user) -> validation -> exception if not valid or generate and return a new id.

* Add tests.

Day 2

Modify project:

* For those who use Fibonacci sequence as an id: start sequence with last 1 – 1, 2...

* Add an ability to store service state

* A service should have an ability to store it's state in an XML file on disk.

* A service should have an ability to store a last generated ID, so the service can continue generating id from where it stops.

* A filename for XML get from App.config.

* Add an ability to log all requests to a service.

* Use System.Diagnostics.BooleanSwitch for enabling/disabling logging.

* Add a possibility to configure log listeners via App.Config: ConsoleTraceListener, and others.

* Add a replication ability for user service nodes:

* Add an ability to have several instances of user service.

* One service should be Master.

* Other services should be Slaves.

* When Master receives a Add or Delete command, it should send a notification to slave services for updating data.

* If Slave receives Add or Delete command it throws an exception.

* All configuration is stored in App.config: a number of services, their types.

* Create a custom section in App.config file, so the number of instances and their role can be changed there.

Day 3


* AppDomain task: https://github.com/epam-lab/dotnetcore/blob/master/Day3/Day3_Domain.sln
