FROM python:3.9-alpine as base

# set environment variables
ENV LANG C.UTF-8
ENV LC_ALL C.UTF-8
ENV PYTHONDONTWRITEBYTECODE 1
ENV PYTHONUNBUFFERED 1

# App
WORKDIR /app

# System
RUN apk add --update --no-cache \
    gcc libc-dev linux-headers postgresql-dev musl-dev zlib zlib-dev \
    geos gdal freetype-dev libffi-dev openssl-dev

# Localtime
RUN cp /usr/share/zoneinfo/Etc/GMT+3 /etc/localtime

COPY auth-server/requirements.txt .

RUN python -m pip install --upgrade pip

# Dev
FROM base AS dev
RUN pip install -r requirements.txt

# # Prod
# FROM nginx:alpine AS nginx
# RUN rm /etc/nginx/conf.d/default.conf
# COPY docker/django/nginx.conf /etc/nginx/conf.d

# FROM base AS prod
# RUN pip install -r requirements.txt

# CMD ["gunicorn", "--bind", "0.0.0.0:8000", "--workers", "4", "project.wsgi"]
