from django.contrib import admin

from .models import Token, SessionMap, Authentication

admin.site.register(Token)
admin.site.register(SessionMap)
admin.site.register(Authentication)
