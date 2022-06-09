# DDNS



#Install

curl -sSL https://get.docker.com | sh

cat << EOF > env.list
endpoint=ovh-eu
application_key=*****
application_secret=*****
consumer_key=*****
record_id=*****
domain_name=mydomain.com
sub_domain_name=sub.mydomain.com
EOF

sudo docker pull reactivethings/ddns:latest
sudo docker run -d --restart unless-stopped --env-file env.list reactivethings/ddns:latest

#Debug 

sudo docker run -it --env-file env.list --entrypoint /bin/bash reactivethings/ddns:latest





