function mostrarFormulario(tipo) {
  document.getElementById('form-cliente').style.display = tipo === 'cliente' ? 'block' : 'none';
  document.getElementById('form-paseador').style.display = tipo === 'paseador' ? 'block' : 'none';
  // Cambia el estado activo de los botones si existen
  if(document.getElementById('btnCliente') && document.getElementById('btnPaseador')) {
    document.getElementById('btnCliente').classList.toggle('active', tipo === 'cliente');
    document.getElementById('btnPaseador').classList.toggle('active', tipo === 'paseador');
  }
}

function agregarMascota() {
  const container = document.getElementById('mascotas-container');
  const index = container.children.length + 1;
  const div = document.createElement('div');
  div.className = 'mascota-form macho'; // Por defecto azul (macho)
  div.innerHTML = `
    <h4>Mascota ${index}</h4>
    <div class="mascota-group-full">
      <label>Muéstranos a tu mascota</label>
      <input type="file" name="mascota_foto_${index}" accept="image/*">
    </div>
    <div class="mascota-group">
      <label>Nombre:</label>
      <input type="text" name="mascota_nombre_${index}" required>
    </div>
    <div class="mascota-group">
      <label>Género:</label>
      <select name="mascota_genero_${index}" onchange="cambiarColorMascota(this)">
        <option value="Macho">Macho</option>
        <option value="Hembra">Hembra</option>
      </select>
    </div>
    <div class="mascota-group">
      <label>Edad:</label>
      <input type="number" name="mascota_edad_${index}" min="0" required>
    </div>
    <div class="mascota-group">
      <label>Vacunas:</label>
      <input type="text" name="mascota_vacunas_${index}" placeholder="Ejemplo: Rabia, Parvovirus">
    </div>
    <div class="mascota-group-full">
      <label>Descripción del comportamiento:</label>
      <textarea name="mascota_comportamiento_${index}" rows="2"></textarea>
    </div>
    <div class="mascota-group-full">
      <label>Observaciones:</label>
      <textarea name="mascota_observaciones_${index}" rows="2"></textarea>
    </div>
    <hr>
  `;
  container.appendChild(div);
}

function cambiarColorMascota(select) {
  const div = select.closest('.mascota-form');
  if (select.value === 'Macho') {
    div.classList.add('macho');
    div.classList.remove('hembra');
  } else {
    div.classList.add('hembra');
    div.classList.remove('macho');
  }
}

// Lógica de login para redirigir según el usuario
document.addEventListener('DOMContentLoaded', function() {
  const loginForm = document.querySelector('.login-form');
  if (loginForm) {
    loginForm.addEventListener('submit', function(e) {
      e.preventDefault();
      const usuario = document.getElementById('usuario').value.trim().toLowerCase();
      // Simulación: si el usuario es "cliente" o "paseador" redirige
      if(usuario === 'cliente') {
        window.location.href = 'usuario.html';
      } else if(usuario === 'paseador') {
        window.location.href = 'paseador.html';
      } else {
        alert('Usuario o contraseña incorrectos');
      }
    });
  }
});