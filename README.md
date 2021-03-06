# IRC

In this kata you implement the Gang Of Four Event Aggregator Pattern [[1](#ref-1), [2](#ref-2)].

## About the Solution

This repository provides 3 solution approaches, each in its own branch:

| Branch                     | Build Status | Description |
| :-----                     | :----------- | :---------- |
| `master`                  | ![Build status of master branch](https://github.com/wonderbird/kata-gof-pattern-eventaggregator-irc/workflows/.NET%20Core/badge.svg?branch=master) | shows how to implement the kata without tools. |
| `ioc-container`          | ![Build status ioc-container branch](https://github.com/wonderbird/kata-gof-pattern-eventaggregator-irc/workflows/.NET%20Core/badge.svg?branch=ioc-container) | uses the Unity.Container inversion of control container to simplify creating the event aggregator and obtaining the reference to it. |
| `prism-eventaggregator` | ![Build status prism-eventaggregator branch](https://github.com/wonderbird/kata-gof-pattern-eventaggregator-irc/workflows/.NET%20Core/badge.svg?branch=prism-eventaggregator) | uses the Unity.Container inversion of control container plus the prism EventAggregator [[4](#ref-4)]. |

## Problem Description

An application shall provide a chat module similar to the original Internet Relay Chat (IRC) [[3](#ref-3)]. It will allow users to send messages to other users. A billing module records the time users have been logged in and the number of messages they have sent. A monitoring module shows how many users are logged in at the same time.

## Intended Application Structure

In order to implement the Event Aggregator Pattern, the program shall be composed of the following components:

![Event Aggregator Pattern](EventAggregatorPattern.png)

- **AuthenticationAppService** allows users to login and logout
- **MessageAppService** allows a user to send a message from one to another user
- **EventAggregator** is the message hub as described by the Event Aggregator Pattern
- **ISubscriber** specifies the method a subscriber has to implement in order to receive events from the EventAggregator
- **BillingAppService** shows login and logout timestamp for each user
- **UserAppService** displays messages sent by another user to the current user
- **MonitoringAppService** shows system status information
- **IMessageView** is a helper class which collects all string messages intended for being displayed in a view to the user interface. This interface can be mocked in a unit test in order to ensure that the messages are rendered correctly. All App Services hold an instance of this interface for communicating with the user interface. This class is not necessary for the Event Aggregator Pattern, it is just used to simplify the application.

## Hint

- Keep the implementation as minimal as possible in order to keep the kata small. Just fulfill the requirements
- Use TDD. Tests first. Red, Green, Refactor.

## Requirements

- The BillingAppService records the user's login timestamp
- The BillingAppService records the user's logout timestamp
- The UserAppService shows when another user has logged in
- The UserAppService shows when another user has logged out
- The UserAppService shows messages sent by users
- The BillingAppService records the number of messages sent by the user
- The MonitoringAppService records the number of users logged in at the same time

## Non-functional Requirements

### Thread Safety

- Allow 10000 users (= UserAppService instances) opening the app at the same time, i.e. 10000 Subscribe requests from different threads to the EventAggregator within 1 second.

### Memory Leak Prevention

- The EventAggregator must not prevent objects from being garbage collected, i.e. use WeakReference to store the subscribers instead of object references.
- Dead WeakReferences must be removed from the subscribers list.

## Finishing Touches

- Avoid duplicated code (use `tools\dupfinder.bat`).
- Fix all static code analysis warnings.
- Check the Cyclomatic Complexity of your source code files. For me, the most complex class has a value of (9 - EventAggregator) and the most complex method has a value of (4 - EventAggregator.Publish). See Visual Studio -> Analyze -> Calculate Code Metrics.

## References

<a name="ref-1">[1]</a> David Starr and others: "Event Aggregator" in "Pluralsight: Design Patterns Library", https://www.pluralsight.com/courses/patterns-library, last visited on Apr. 23, 2020.

<a name="ref-2">[2]</a> Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides: "Design Patterns: Elements of Reusable Object-Oriented Software", Addison Wesley, 1994, pp. 151ff, [ISBN 0-201-63361-2](https://en.wikipedia.org/wiki/Special:BookSources/0-201-63361-2).

<a name="ref-3">[3]</a> Wikipedia: "Internet Relay Chat", https://en.wikipedia.org/wiki/Internet_Relay_Chat, last visited on Apr. 23, 2020.

<a name="ref-4">[4]</a> .NET Foundation and Contributors: "Prism Library: EventAggregator", https://prismlibrary.com/docs/event-aggregator.html, last visited on May 26, 2020.