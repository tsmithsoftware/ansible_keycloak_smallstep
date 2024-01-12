# ansible_keycloak_smallstep

## Description
This project is aimed at configuring Keycloak with a web application. The SSL certificates are generated by Smallstep, and the configuration is performed using Ansible.

## Getting started
This project requires `ansible`, `ansible-playbook`, `docker` and `docker-compose` installed.

## Installation and running
From the root directory:

`docker-compose up`

Once the containers are all running:

`ansible-playbook -i ./ansible/inventory ./ansible/playbook -v -e 'ansible_python_interpreter=/usr/bin/python3'`

The playbook initialises the containers with the correct SSL certificates. 
The playbook is not idempotent - once the playbook is run once, the certificates cannot be re-generated using the same containers/playbook.
The containers need to be torn down and destroyed using `docker-compose down`, then re-scaffolded using `docker-compose up` before the playbook can be re-run.
