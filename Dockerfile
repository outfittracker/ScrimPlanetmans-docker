FROM debian:11
LABEL package.date=2022-11-12
WORKDIR /app

RUN apt-get update -y
RUN apt-get install -y wget
RUN wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb
RUN apt-get update -y
RUN apt-get install -y dotnet-sdk-3.1

ADD squittal.ScrimPlanetmans.App/bin/Debug/netcoreapp3.1/ /app
ADD sql_adhoc /app/sql_adhoc
ADD rulesets /app/rulesets
ADD squittal.ScrimPlanetmans.App/wwwroot /app/wwwroot
COPY DockerData/appsettings.json /app/appsettings.json
COPY DockerData/start.sh /app/start.sh
COPY DockerData/wait-for-it.sh /app/wait-for-it.sh

EXPOSE 5000
EXPOSE 5001

CMD ["bash","/app/wait-for-it.sh","-t","60", "planetmans-db:1433", "--", "bash", "/app/start.sh"]