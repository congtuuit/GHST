docker build -f deploy.dockerfile -t ghst_app .

docker run -d -p 8080:80 --name ghst_app_container ghst_app
