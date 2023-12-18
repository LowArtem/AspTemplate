FROM nginx:1.25.3

COPY /nginx/conf.d /etc/nginx/conf.d
COPY /nginx/proxy.locations /etc/nginx/conf.d/locations/application-backend.locations
