# Identity Monad
The Identity Monad is one of the simplest types of monads used in functional programming. It simply applies the monad interface without adding any computational context or effects. The Identity Monad can be seen as a way to provide a uniform interface for operations that do not require any special handling. Essentially, it wraps a value and allows it to be used within the monadic framework, making it easier to write generic functions and compose operations.

## Install
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
    "com.amariliscreation.rws_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/RWS/Assets/Plugins"
  ...
  }
```

## Example
We have prepared [sample code](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/Identity/Assets/IdentityMonadExample.cs), so please take a look there.

## License
This library is under the MIT License.