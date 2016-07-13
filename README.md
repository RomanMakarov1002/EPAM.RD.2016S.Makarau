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
