# I went ys

Iwentys - это прототип системы для геймификации процесса обучения студентов. На данный момент проект заморожен, большая часть наработок (и разработчиков) ушли в https://github.com/kysect/Shreks. Shreks - это реализация одного из модулей Iwentys.

Iwentys - это платформа для разработки микросервисной системы поддержки факультета.

На данный момент этот репозиторий содержит два сервиса - основной Iwentys и Iwentys.EntityManager. Во второй сервис вынесены CRUD'ы для работы со студентами, группами и прочее. Это позволит в других проектах переиспользовать функционал, а в дальнейшем в продакшене другим сервисам не думать о получении данных о студентах, авторизации и прочее.

## How to build and run

Во время запуска основного сервиса (Iwentys.WebService.Api) требуется поднять Iwentys.EntityManager.WebApi т.к. во время старта происходит синк данных между базами:

```
dotnet run --project Iwentys.EntityManager/Source/Iwentys.EntityManager.WebApi/Iwentys.EntityManager.WebApi.csproj
```

Нужно убедиться, что сервис запустился и можно запускать основной:

```
dotnet run --project Source/WebService/Iwentys.WebService.Api/Iwentys.WebService.Api.csproj
```

Порты:
- Iwentys.EntityManager.WebApi - 7276
- Iwentys.WebService.Api - 5001
