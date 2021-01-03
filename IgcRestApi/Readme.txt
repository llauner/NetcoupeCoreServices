#Deploy on GCP / Cloud Run

- Build docker image (right click on dockerfile / build)
- docker tag igcrestapi eu.gcr.io/igcheatmap/igcrestapi:latest
- docker push eu.gcr.io/igcheatmap/igcrestapi
- Deploy new revision in GCP: https://console.cloud.google.com/run?project=igcheatmap
