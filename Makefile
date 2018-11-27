filter= ""

.PHONY: restore
restore:
	dotnet restore wallet.sln

.PHONY: build
build: stop restore
	docker-compose build

.PHONY: up
up: prune
	docker-compose up -d

.PHONY: stop
stop:
	docker-compose stop

.PHONY: down
down:
	docker-compose down

.PHONY: attach
attach: up
	docker-compose exec wallet.api bash

.PHONY: mysql
mysql: up
	docker-compose exec database mysql -u root -h database -psecret --database wallet
    
.PHONY: migrate
migrate: up
	cd wallet.repository.mysql && dotnet ef database update

.PHONY: migrate-script
migrate-script: up
	cd wallet.repository.mysql && dotnet ef migrations script

.PHONY: prune
prune: stop
	docker system prune

.PHONY: release
release:
	git tag -a $(version) -m "$(comment)"
