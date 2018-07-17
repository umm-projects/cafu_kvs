# cafu_kvs

## What

- Simple key value data store system

## Requirement

- cafu\_core

## Install

```shell
yarn add "umm/cafu_kvs#^1.0.0"
```

## Usage

```
var savePath = UnityEngine.Application.persistentDataPath + "/default.kv";
var repository = new DefaultKeyValueRepository.Factory().Create(savePath);

// Load & Get
repository.Load();
var entity = repository.GetEntity<SampleEntity>("key");

// Set & Save
repository.SetEntity<SampleEntity>("key", entity);
repository.Save();

```

## License

Copyright (c) 2018 Takuma Maruyama

Released under the MIT license, see [LICENSE.txt](LICENSE.txt)

