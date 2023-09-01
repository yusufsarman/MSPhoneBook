# MSPhoneBook

### Senaryo

>  Birbirleri ile haberleşen minimum iki microservice'in olduğu bir yapı tasarlayarak, basit bir telefon rehberi uygulaması oluşturulması sağlanacaktır.
##### Beklenen işlevler:

- Rehberde kişi oluşturma
- Rehberde kişi kaldırma
- Rehberdeki kişiye iletişim bilgisi ekleme
- Rehberdeki kişiden iletişim bilgisi kaldırma
- Rehberdeki kişilerin listelenmesi
- Rehberdeki bir kişiyle ilgili iletişim bilgilerinin de yer aldığı detay bilgilerin 
getirilmesi
- Rehberdeki kişilerin bulundukları konuma göre istatistiklerini çıkartan bir rapor 
talebi
- Rehberdeki kişilerin bulundukları konuma göre istatistiklerini çıkartan bir rapor 
talebi
- Sistemin oluşturduğu raporların listelenmesi
- Sistemin oluşturduğu bir raporun detay bilgilerinin getirilmesi

#
- Healthcheck durumu görmek için ; 
> Contact Healthcheck : http://localhost:5002/contact-service-healthcheck
```
{
  "status": "Healthy",
  "totalDuration": "00:00:07.1212011",
  "entries": {
    "Postgresql ContactApi Healtcheck": {
      "data": {
        
      },
      "duration": "00:00:00.1633020",
      "status": "Healthy",
      "tags": [
        "postgreContactApi"
      ]
    },
    "RabbitMQ HealthCheck": {
      "data": {
        
      },
      "description": "None of the specified endpoints were reachable",
      "duration": "00:00:07.1079044",
      "exception": "None of the specified endpoints were reachable",
      "status": "Healthy",
      "tags": [
        "rabbitmq"
      ]
    }
  }
}
```
> Report Healthcheck : http://localhost:5003/report-service-healthcheck
```
{
  "status": "Healthy",
  "totalDuration": "00:00:07.0944033",
  "entries": {
    "Postgresql ReportApi Healtcheck": {
      "data": {
        
      },
      "duration": "00:00:00.1630750",
      "status": "Healthy",
      "tags": [
        "postgreReportApi"
      ]
    },
    "RabbitMQ HealthCheck": {
      "data": {
        
      },
      "description": "None of the specified endpoints were reachable",
      "duration": "00:00:07.0909510",
      "exception": "None of the specified endpoints were reachable",
      "status": "Healthy",
      "tags": [
        "rabbitmq"
      ]
    }
  }
}
``` 
 ## 
>Contact Api swagger url = http://localhost:5002/swagger/index.html
##
>Report Api Swagger url = http://localhost:5003/swagger/index.html
 ## Kullanılan Teknolojiler 
> .net Core 7, EFCore, PostgreSql, RabbitMq, HealthCheck , Api Versioning
