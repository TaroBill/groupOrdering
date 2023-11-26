group_ordering_image_name = group_ordering
mysql_image_name = group_ordering_mysql

group_ordering_path = ./groupOrdering
mysql_path = ./mysql

docker_build:
	docker build -t $(group_ordering_image_name) -f $(group_ordering_path)/Dockerfile . 
	docker build -t $(mysql_image_name) -f $(mysql_path)/Dockerfile ./mysql

docker_start:
	docker-compose up -d
	
docker_stop:
	docker-compose down

docker_rmi:
	docker rmi $(group_ordering_image_name)
	docker rmi $(mysql_image_name)
docker_rm_data:
	sudo rm -rf /group_ordering_mysql
	@echo success

build_and_run: docker_build docker_start
stop_and_rm: docker_stop docker_rmi docker_rm_data
