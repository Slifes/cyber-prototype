from django.urls import path

from . import views

urlpatterns = [
    path('character/create/', views.character_create, name="character-create"),
]
