# IOC and Class Wrapping Examples
<sub>Dan Grecoe a Microsoft Employee</sub>

After playing with Python for a while, I really got used to being able to create decorators that I could put around functions. This allowed the code to not alter the underlying interface but extend it with specialized logging, timing, pre-setup of systems that were needed. 

Recently, I'm moving back into the .NET world and realized that performing that wrapping was much more complex and less generic than Python. 

Because of that I wanted to start trying to build something that would be more generic. As part of that I was also interested in a simplified inversion of control (IOC) pattern that could be used as well. 

Most developers understand the importance of IOC for longer term development projects. 

This led me to create a few utilities appropriately placed in teh Utils .NET Core class library. In fact, all the code here has been written in .NET Core so it is relatively platform agnostic. 

The projects in this repo build up, so it's probably useful to follow along in order (unless you specifically are only interested in IOC or Class Wrapping)

## IOCFactory
This program uses the IOC pattern by creating a factory like object in which you can register classes, retrieve instances of existing classes, or create new ones that use injection. 

Injection works by first adding instances of specific classes (or interfaces) to the factory. Next you ask the factory to create an instance of a regular or generic class that relies on another class (already created) being injected.

The caveat is that injection of specific classes occurs only through the class constructor.

## Class Wrapping
This is more of a pattern. To get the same sort of functionality that I wanted with Python decorators, you follow a more complex pattern. The general flow is

1. Create an interface
2. Derive your true class, the one with the functionality, from the interface.
3. Create a "wrapper" class that also derives from the interface but takes the class from step 2 into it. 

Filling in the class from step 3 is straighforward but allows you to call regular async functions, or functions donoted with Task<>. 

That is all wrapped under the hood for you in a class called Mapper which is where the wrapping is happening and logging/timing/etc is placed around your original class (step2).

## Complete
This is a complete, although contrived, example using both patterns above - IOC and Class Wrapping - for a complete example of using these pieces of functionality. 

In this example there is a base class of ISourceReader and you have two types of it. ISystemReader and IWebReader in which you'd like to use IOC as well as wrapping them to log out calling information. 

