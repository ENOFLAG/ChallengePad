# ChallengePad [![Docker](https://img.shields.io/docker/v/enoflag/challengepad?sort=semver)](https://hub.docker.com/r/enoflag/challengepad) ![ChallengePad CI](https://github.com/ENOFLAG/ChallengePad/workflows/ChallengePad%20CI/badge.svg) ![](https://tokei.rs/b1/github/ENOFLAG/ChallengePad)


## Installation
```yaml
version: '3'

services:
  challengepad:
    image: enoflag/challengepad:latest
    environment:
      - DATABASE=Server=postgres; Port=5432;Database=ChallengePadDb;User Id = docker; Password=docker;Timeout=15;SslMode=Disable;
      - REDIS=redis
      - TITLE=ENOFLAG ChallengePad
      - PAD_PREFIX=https://codimd.example.com/suchpadmuchwow
      - PAD_SUFFIX=?edit
      - OAUTH_CLIENT_ID=[...]
      - OAUTH_CLIENT_SECRET=[...]
      - OAUTH_AUTHORIZATION_ENDPOINT=[...]
      - OAUTH_TOKEN_ENDPOINT=[...]
      - OAUTH_USER_INFORMATION_ENDPOINT=[...]
      - OAUTH_SCOPE=[...]
    ports:
      - 80:80
    volumes:
      - "./uploads:/app/Uploads"
  redis:
    image: redis
  postgres:
    image: postgres
    environment:
      POSTGRES_USER: docker
      POSTGRES_PASSWORD: docker
```
