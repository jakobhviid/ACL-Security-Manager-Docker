# About
An image for managing ACL in the CFEI stack, the image consists of two parts.

The first part is the Kafka Security Manager written by simplesteph (https://github.com/simplesteph/kafka-security-manager). It watches an acls.csv file. In this file access control rules can be written, this way it's possible to limit a user's access to the kafka cluster. For example it's possible to limit both read and write access to specific topics.
This manager requires a kerberos keytab for the zookeeper in which acls are written in the kafka / zookeeper stack. 

The second part is an API for writing new rules to the acls.csv file remotely via http endpoints. The API works by entering an API Key during startup. This API Key has to be provided in order to be able to write new rules.

# How to use
This docker-compose file show the deployment of the container

```
version: "3"

services:
  acl-manager:
    image: cfei/acl-security-manager
    container_name: acl-manager
    ports:
      - 9000:9000
    environment:
      AUTHORIZER_ZOOKEEPER_CONNECT: <<ZOOKEEPER_URL>>:2181
      ACL_API_PORT: 9000
      ACL_API_KEY: cfei-key
      ACL_KERBEROS_API_URL: "<<CFEI_KERBEROS_API_URL>>/get-keytab"
      ACL_KERBEROS_API_ZOOKEEPER_USERNAME: <<ZOOKEEPER_KEYTAB_USERNAME>>
      ACL_KERBEROS_API_ZOOKEEPER_PASSWORD: <<ZOOKEEPER_KEYTAB_PASSWORD>>
      ACL_KERBEROS_REALM: CFEI.SECURE
      ACL_KERBEROS_PUBLIC_URL: <<CFEI_KERBEROS_API_URL>>

```

## API Endpoints
**Terms explained**
A 'superuser' is a user which has access to everything in the kafka cluster. 
A 'principal' is the username of a kerberos keytab

In the following examples the host is example.com and the port is 9000.

### POST example.com:9000/new-super-user
This endpoint creates a superuser. The api key provided during the container setup is required here.
##### Example Request (JSON)
```
{
	"apiKey": "cfei-key",
    "principalName": "examplecfeiuser"
}
```
##### Returns one of the following
- 403: "API key incorrect"
- 201: "Super user successfully created"

### POST example.com:9000/access-control
This endpoint writes a new rule to the access control list.
**TODO**

### GET example.com:9000/access-control
This endpoint fetches all rules for a single user (principal name)
**TODO**

### PATCH example.com:9000/access-control
This endpoint updates a single rule
**TODO**

### DELETE example.com:9000/access-control
This endpoint deletes a single rule
**TODO**

# Configuration

- `AUTHORIZER_ZOOKEEPER_CONNECT`: The DNS-resolvable zookeeper url to authorize and store acls in. This environment variable supports a comma seperated list of zookeeper urls. Required.

- `ACL_API_PORT`: The port on which the API will listen for connections. Required.

- `ACL_API_KEY`: The API key to use. This can be used for everything in this image, so store it safely! Required.

- `ACL_KERBEROS_API_URL`: The URL to use when the image fetches the zookeeper keytab from a kerberos server. The URL has to point to an HTTP POST Endpoint. The image will then supply the values of 'ACL_KERBEROS_API_ZOOKEEPER_USERNAME' and 'ACL_KERBEROS_API_ZOOKEEPER_PASSWORD' to the POST request. Required if a keytab is not provided through volumes.

- `ACL_KERBEROS_API_ZOOKEEPER_USERNAME`: The username to use when fetching the zookeeper keytab. Required if a keytab is not provided through volumes.

- `ACL_KERBEROS_API_ZOOKEEPER_PASSWORD`: The password to use when fetching the zookeeper keytab. Required if a keytab is not provided through volumes.

- `ACL_KERBEROS_REALM`: The kerberos realm which will be used in keytabs. Required.

- `KERBEROS_HOST_DNS`: The DNS-resolvable hostname to use which the kerberos server is identified by. It should be set to the FQDN of the server on which kerberos is running on. Required.

- `ACL_KERBEROS_PUBLIC_URL`: Public DNS of the kerberos server to use. Required.

# Volumes

- `/conf/`: Two important files reside in this directory. Both the zookeeper keytab which is used to write acls and also the acls.csv file. You can provide your own keytab and acls.csv file which the image will use instead of creating it's own.
  
- TODO: API Logging

# Security
TODO: Add support for https