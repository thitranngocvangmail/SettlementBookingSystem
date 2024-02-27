# SettlementBookingTest

## Versions

BookingController` provides an implementation using the mediator pattern and the data is stored in an [Interval Tree](https://en.wikipedia.org/wiki/Interval_tree) for O(log m + n) access.
This implementatrion also provides unit tests with a mocked repository.

## How to execute
curl -X POST "https://localhost:44355/Booking" -H  "accept: text/plain" -H  "Content-Type: application/json" -d "{\"name\":\"Van Thi\",\"bookingTime\":\"09:10\"}"

"name" field is required
"bookingTime" field is required with 24-hour and minute format: hh:mm, it should also be within working hour from 9:00 am to 4:59 pm.

Technical implementation highlights:
1. ASP.NET Core API with Mediator 
2. Clean architecture and domain driven design so there separate layers for Application & Domain.
3. Data Persistent using relational DB and EF Core as ORM tool, for quick development purpose, it is using InMemory DB provider of EF core package.
4. Unit Test with xUnit and Moq for Application & Domain layers
5. Valilation: 
   + Simple Dto validation in the Application layer using FluentValidator
   + Business validation is done in the Domain layer, which also cover re-validation from the Application Layer to ensure data correctness before inserting them into the persistent storage.