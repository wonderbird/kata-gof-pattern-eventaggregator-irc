# IRC

In this kata you implement the Gang Of Four Event Aggregator Pattern [[1](#ref-1), [2](#ref-2)].

## Problem Description

tbd

Draft idea

An application shall provide a chat module similar to the original Internet Relay Chat (IRC) [[3](#ref-3)]. It will allow users to see how many other users are using the system. Users have to pay for the time they were logged in and for the number of messages they have sent.

## Requirements

tbd

Draft:
- For a billing view record the user's login timestamp
- For a billing view record the user's logout timestamp
- For a billing view record the number of messages sent by the user
- Allow user to see how many other users are currently logged into the chat
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