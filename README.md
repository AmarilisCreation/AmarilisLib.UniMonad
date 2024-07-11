# AmarilisLib.UniMonad
This library is for using monads in Unity

## [IdentityMonad](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/Identity/Assets/Plugins)
The Identity Monad is one of the simplest types of monads used in functional programming. It simply applies the monad interface without adding any computational context or effects. The Identity Monad can be seen as a way to provide a uniform interface for operations that do not require any special handling. Essentially, it wraps a value and allows it to be used within the monadic framework, making it easier to write generic functions and compose operations.

## [OptionMonad](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/Option/Assets/Plugins)
The Option Monad, also known as the Maybe Monad in some programming languages, is a type of monad used to represent computations that might fail or return no value. It encapsulates an optional value; the computation can result in either a value (Just) or no value (None). The Option Monad allows for chaining operations on optional values without needing to check explicitly for the presence or absence of a value at each step. This leads to more concise and readable code when handling potentially missing values.

## [EitherMonad](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/Either/Assets/Plugins)
The Either Monad is a type of monad used in functional programming to handle computations that may result in two different types of values, typically representing success or failure. The Either Monad encapsulates a value that can be either a "Left" or a "Right". By convention, "Left" is used to represent an error or failure, while "Right" represents a successful computation. This allows for chaining operations where each step can handle potential errors gracefully without using exceptions. The Either Monad simplifies error handling and makes the code more expressive and easier to maintain.

## [TryMonad](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/Try/Assets/Plugins)
The Try Monad is a type of monad used in functional programming to handle computations that may throw exceptions. It encapsulates a computation that can either result in a successful value or an exception, providing a way to handle errors without using traditional try-catch blocks. The Try Monad has two primary states: Success, which contains the successfully computed value, and Failure, which contains the exception. This allows for chaining operations while propagating errors in a controlled manner, leading to cleaner and more maintainable error handling in code.

## [IOMonad](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/IO/Assets/Plugins)
The IO Monad is a type of monad used in functional programming to handle input/output operations in a pure functional way. Since pure functions cannot have side effects, the IO Monad provides a way to describe and sequence side effects while maintaining functional purity. The IO Monad encapsulates actions that interact with the external world (e.g., reading a file, printing to the console) and allows these actions to be composed and sequenced within the monadic framework. This ensures that side effects are controlled and predictable, making it easier to reason about and maintain the code.

## [ReaderMonad](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/Reader/Assets/Plugins)
The Reader Monad, also known as the Environment Monad, is a type of monad used in functional programming to handle read-only shared environments or configurations. It allows functions to access shared data without passing it explicitly through each function call. The Reader Monad encapsulates a computation that depends on an external environment, providing a way to inject this environment into the computation chain. This leads to cleaner and more maintainable code by avoiding the need to pass the environment explicitly and by enabling better separation of concerns.

## [WriterMonad](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/Writer/Assets/Plugins)
The Writer Monad is a type of monad used in functional programming to handle computations that produce a log or auxiliary output along with the main result. It allows functions to output additional data, such as logs or debug information, without modifying the primary logic of the computation. The Writer Monad encapsulates a computation paired with a monoidal value, usually representing the log, and provides operations to combine logs and results. This leads to cleaner code by separating the concerns of computation and logging, and it ensures that the logging context is managed consistently throughout the computation.

## [StateMonad](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/State/Assets/Plugins)
The State Monad is a type of monad used in functional programming to handle stateful computations in a functional way. It encapsulates a state along with a computation, allowing functions to read and modify the state without explicitly passing it through each function call. The State Monad provides a mechanism to thread state through a sequence of computations while maintaining functional purity. This results in cleaner and more modular code, as it separates state management from the core logic of the computations.

## [RWSMonad](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/RWS/Assets/Plugins)
The RWS Monad is a composite monad in functional programming that combines the functionalities of the Reader, Writer, and State monads. It allows a function to read from a shared environment (Reader), produce a log or auxiliary output (Writer), and manage state (State) within a single monadic context. The RWS Monad encapsulates computations that require these three aspects, providing a unified interface for operations that need to access shared configuration data, accumulate logs, and modify state. This leads to more expressive and maintainable code by centralizing the management of these different concerns in a consistent and cohesive manner.

## UniMonad Full Install
### Using UnityPackageManager
Find the manifest.json file in the Packages folder of your project and edit it to look like this.
```
"scopedRegistries": [
  "dependencies": {
    "com.amariliscreation.common": "https://github.com/AmarilisCreation/AmarilisLib.Common.git?path=src/Common/Assets/Plugins",
    "com.amariliscreation.identity_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/Identity/Assets/Plugins",
    "com.amariliscreation.option_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/Option/Assets/Plugins",
    "com.amariliscreation.either_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/Either/Assets/Plugins",
    "com.amariliscreation.try_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/Try/Assets/Plugins",
    "com.amariliscreation.io_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/IO/Assets/Plugins",
    "com.amariliscreation.reader_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/Reader/Assets/Plugins",
    "com.amariliscreation.writer_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/Writer/Assets/Plugins",
    "com.amariliscreation.state_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/State/Assets/Plugins",
    "com.amariliscreation.rws_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/RWS/Assets/Plugins",
  ...
  }
```