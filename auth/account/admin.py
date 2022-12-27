from django.contrib import admin

from .models import Token, SessionMap

admin.site.register(Token)
admin.site.register(SessionMap)
