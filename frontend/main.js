const functionApi = 'http://localhost:7071/api/GetResumeCounter';

async function updateCounter() {
  const el = document.getElementById('counter');
  if (!el) { console.warn('counter element not found'); return; }

  try {
    const resp = await fetch(functionApi, { cache: 'no-store' });
    if (!resp.ok) throw new Error(`${resp.status} ${resp.statusText}`);
    el.textContent = (await resp.text()).trim() || '0';
    console.log('Counter updated');
  } catch (err) {
    console.error('Counter fetch failed:', err);
    el.textContent = '—';
  }
}

updateCounter();
