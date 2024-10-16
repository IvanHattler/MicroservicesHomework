# �������� ������ �������

����������� 4 ������������, ����������� � Docker-�����������, � docker-compose.yml ��� ����������� ������� ���� ��������.


1. `MicroservicesHomework.ServiceA` - ������, ���������� �� ������� �������, ��� � ��������� ����� ��������� ��������� ������������� �������, � ���������� � ����������� ������ �� ����������� ����� �������������� � ����� �������.
1. `MicroservicesHomework.ServiceB` - ������, ���������� �� ������� ������, ��� � ��������� ����� ��������� ��������� ������������� �������, � ���������� � ����������� ������ �� ����������� ����� �������������� � ����� �������.
1. `MicroservicesHomework.Orchestrator` - �����������, ��� ������� ������ ��� ������� (���� ��� ������� �� ������, ������ ��� ������� �� ������), � ���������� ������ �������������� � �������� � ��������������� �������.
1. `MicroservicesHomework.Notifications` - ������, ����������� ��������� � �������� �������� �������.

��� ������ ������������� ���� �� �����, ���� ���������� � ���������� ���������, ������:
```yaml
microserviceshomework.orchestrator:
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_HTTP_PORTS=8080
      - notifications_url=http://notifications:8080
      - servicea_url=http://servicea:8080
      - serviceb_url=http://serviceb:8080
```

�� ����� ������ ������� � ������� ����������� ����������� ����� ���������:

![notifications](https://github.com/IvanHattler/MicroservicesHomework/blob/master/NotificationsImage.png?raw=true)



