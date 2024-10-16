# ќписание работы системы

–азработано 4 микросервиса, запускаемых в Docker-контейнерах, и docker-compose.yml дл€ совместного запуска всех сервисов.


1. `MicroservicesHomework.ServiceA` - сервис, отвечающий за закупку стульев, раз в некоторое врем€ провер€ет следующий идентификатор клиента, и отправл€ет в оркестратор запрос на перемещение этого идентификатора в конец очереди.
1. `MicroservicesHomework.ServiceB` - сервис, отвечающий за закупку молока, раз в некоторое врем€ провер€ет следующий идентификатор клиента, и отправл€ет в оркестратор запрос на перемещение этого идентификатора в конец очереди.
1. `MicroservicesHomework.Orchestrator` - оркестратор, при запуске создаЄт две очереди (одна дл€ очереди на стуль€, втора€ дл€ очереди на молоко), и отправл€ет первые идентификаторы в очеред€х в соответствующие сервисы.
1. `MicroservicesHomework.Notifications` - сервис, принимающий сообщени€ о попытках заказать продукт.

ƒл€ вызова микросервисов друг из друга, пути передаютс€ в переменных окружени€, пример:
```yaml
microserviceshomework.orchestrator:
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_HTTP_PORTS=8080
      - notifications_url=http://notifications:8080
      - servicea_url=http://servicea:8080
      - serviceb_url=http://serviceb:8080
```

¬о врем€ работы системы в сервисе нотификации логгируютс€ такие сообщени€:

![notifications](https://github.com/IvanHattler/MicroservicesHomework/blob/master/NotificationsImage.png?raw=true)



