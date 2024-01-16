FROM albericwalsh/ecommerce:WEB-ecomerce
FROM ubuntu
RUN apt update && apt -y install locales && locale-gen en_US.UTF-8
ENV LANG en_US.UTF-8
ENV LANGUAGE en_US:en
ENV LC_ALL en_US.UTF-8
COPY ./default.conf /etc/nginx/conf.d/default.conf
WORKDIR /docker-entrypoint.d
ADD dotnet_install.sh .
RUN chmod +x dotnet_install.sh
RUN ./dotnet_install.sh
ADD ./start.sh .
EXPOSE 80
WORKDIR /app
COPY ./Test_Projet.csproj .
#RUN dotnet restore
COPY ./Program.cs .
COPY ./SQLRequest.cs .
#RUN dotnet publish -c Release -o out
#ENTRYPOINT ["dotnet", "out/Test_Projet.dll"]
#CMD ["/start.sh"]
CMD ["nginx", "-g", "daemon off;"]
CMD ["dotnet", "run"]