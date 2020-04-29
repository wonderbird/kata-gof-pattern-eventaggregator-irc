# IRC

In this kata you implement the Gang Of Four Event Aggregator Pattern [[1](#ref-1), [2](#ref-2)].

## Problem Description

tbd

Draft idea

An application shall provide a chat module similar to the original Internet Relay Chat (IRC) [[3](#ref-3)]. It will allow users to see how many other users are using the system. Users have to pay for the time they were logged in and for the number of messages they have sent.

## Program structure

The program shall be composed of the following components:

- **AuthenticationAppService** allows users to login and logout
- **BillingAppService** records login and logout timestamp for each user
- **UserAppService** manages information displayed to the user
- **IMessageView** sends string messages intended for being displayed in a view to the rendering engine. This interface can be mocked in a unit test in order to ensure that the messages are rendered correctly. All App Services hold an instance of this interface for communicating with the user interface.

## Requirements


Draft:
- For a billing view record the user's login timestamp
- For a billing view record the user's logout timestamp
- Allow a user to see when another user has logged in
- Allow a user to see when another user has logged out
- For a billing view record the number of messages sent by the user
- For a monitoring view record the number of users logged in at the same time
- For a monitoring view record the number of messages sent in the past time interval (for demo: 1 second)

## Suggested Test Cases

tbd

## Finishing Touches

- Avoid duplicated code (use `tools\dupfinder.bat`).
- Fix all static code analysis warnings.
- Check the Cyclomatic Complexity of your source code files. For me, the most complex class hat a value of (tbd) and the most complex method has a value of (tbd). See Visual Studio -> Analyze -> Calculate Code Metrics.

## References

<a name="ref-1">[1]</a> David Starr and others: "Event Aggregator" in "Pluralsight: Design Patterns Library", https://www.pluralsight.com/courses/patterns-library, last visited on Apr. 23, 2020.

<a name="ref-2">[2]</a> Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides: "Design Patterns: Elements of Reusable Object-Oriented Software", Addison Wesley, 1994, pp. 151ff, [ISBN 0-201-63361-2](https://en.wikipedia.org/wiki/Special:BookSources/0-201-63361-2).

<a name="ref-3">[3]</a> Wikipedia: "Internet Relay Chat", https://en.wikipedia.org/wiki/Internet_Relay_Chat, last visited on Apr. 23, 2020.