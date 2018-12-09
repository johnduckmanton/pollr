# Docker

docker run -it -p 8080:80 -e PollrDatabase="Server=tcp:demodb-jrd.database.windows.net,1433;Initial Catalog=pollr;Persist Security Info=False;User ID=duckmanj;Password=z9uWLk3\$R6sqFW;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" pollr.api:latest
docker build --rm -f "Dockerfile" -t pollr.api:latest .
